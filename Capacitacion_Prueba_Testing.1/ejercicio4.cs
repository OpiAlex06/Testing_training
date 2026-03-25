using System;

// =============================================================================
// TÉCNICA 4: Reemplazar Condicional con Polimorfismo
//
// PROBLEMA:
//   Hay if/else o switch que ejecutan código diferente según el "tipo" de
//   objeto. Si existe el mismo switch en varios métodos (velocidad, sonido,
//   comportamiento...), agregar un tipo nuevo implica modificar TODOS ellos.
//
// SOLUCIÓN:
//   Cada rama del switch se convierte en una subclase que sobreescribe el
//   método con su propio comportamiento. C# llama al método correcto
//   automáticamente según el tipo real del objeto en tiempo de ejecución.
//
// POR QUÉ ES NO TRIVIAL:
//   Requiere identificar la jerarquía correcta, usar clases abstractas,
//   el modificador "override" y reorganizar la lógica existente.
// =============================================================================


// ── ANTES ────────────────────────────────────────────────────────────────────
class PajaroAntes
{
    public const string Africano = "africano";
    public const string Europeo = "europeo";
    public const string NoruegoAzul = "noruego_azul";

    private readonly string _tipo;
    private readonly int _numeroDeCocos;
    private readonly double _voltaje;
    private readonly bool _clavado;

    public PajaroAntes(string tipo, int numeroDeCocos = 0,
                       double voltaje = 0, bool clavado = false)
    {
        _tipo = tipo;
        _numeroDeCocos = numeroDeCocos;
        _voltaje = voltaje;
        _clavado = clavado;
    }

    // Este switch debe actualizarse cada vez que se agrega un tipo de pájaro
    public double ObtenerVelocidad()
    {
        return _tipo switch
        {
            Europeo => VelocidadBase(),
            Africano => Math.Max(0, VelocidadBase() - FactorCarga() * _numeroDeCocos),
            NoruegoAzul => _clavado ? 0 : Math.Min(24, _voltaje / 10),
            _ => throw new ArgumentException($"Tipo desconocido: {_tipo}")
        };
    }

    private double VelocidadBase() => 12;
    private double FactorCarga() => 0.5;
}


// ── DESPUÉS ───────────────────────────────────────────────────────────────────
abstract class Pajaro
{
    // "virtual" permite que las subclases sobreescriban este método
    protected virtual double VelocidadBase() => 12;

    // "abstract" obliga a las subclases a implementar su propia velocidad
    public abstract double ObtenerVelocidad();
}

class PajaroEuropeo : Pajaro
{
    public override double ObtenerVelocidad() => VelocidadBase();
}

class PajaroAfricano : Pajaro
{
    private readonly int _numeroDeCocos;

    public PajaroAfricano(int numeroDeCocos)
    {
        _numeroDeCocos = numeroDeCocos;
    }

    public override double ObtenerVelocidad()
    {
        const double factorCarga = 0.5;
        return Math.Max(0, VelocidadBase() - factorCarga * _numeroDeCocos);
    }
}

class PajaroNoruegoAzul : Pajaro
{
    private readonly double _voltaje;
    private readonly bool _clavado;

    public PajaroNoruegoAzul(double voltaje, bool clavado)
    {
        _voltaje = voltaje;
        _clavado = clavado;
    }

    public override double ObtenerVelocidad()
    {
        return _clavado ? 0 : Math.Min(24, _voltaje / 10);
    }
}

