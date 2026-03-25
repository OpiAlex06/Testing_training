using System;

// =============================================================================
// TÉCNICA 2: Reemplazar Código de Tipo con Subclases
//
// PROBLEMA:
//   Una clase usa un string o constante ("ingeniero", "gerente") para controlar
//   su comportamiento con if/else o switch. Cada vez que agregamos un tipo
//   nuevo, debemos modificar TODOS los switch del sistema.
//   Esto viola el principio Open/Closed: el código no está cerrado a modificación.
//
// SOLUCIÓN:
//   Crear una subclase por cada valor posible del "tipo". El polimorfismo de C#
//   se encarga de llamar al método correcto según la subclase real del objeto.
//   Para agregar un tipo nuevo solo se crea una nueva subclase, sin tocar nada.
//
// POR QUÉ ES NO TRIVIAL:
//   Hay que rediseñar la jerarquía de clases, usar "abstract" y "override",
//   y agregar un Factory Method para centralizar la creación de objetos.
// =============================================================================


// ── ANTES ────────────────────────────────────────────────────────────────────
class EmpleadoAntes
{
    // Constantes que representan los "tipos": esto es lo problemático
    public const string Ingeniero = "ingeniero";
    public const string Vendedor = "vendedor";
    public const string Gerente = "gerente";

    public string Nombre { get; }
    private readonly string _tipoEmpleado; // campo que guarda el tipo

    public EmpleadoAntes(string nombre, string tipoEmpleado)
    {
        Nombre = nombre;
        _tipoEmpleado = tipoEmpleado;
    }

    // Switch que hay que actualizar cada vez que aparece un nuevo tipo
    public int ObtenerBono()
    {
        return _tipoEmpleado switch
        {
            Ingeniero => 5000,
            Vendedor => 3000,
            Gerente => 8000,
            _ => throw new ArgumentException($"Tipo desconocido: {_tipoEmpleado}")
        };
    }

    public int ObtenerDiasVacaciones()
    {
        return _tipoEmpleado switch
        {
            Ingeniero => 20,
            Vendedor => 15,
            Gerente => 25,
            _ => throw new ArgumentException($"Tipo desconocido: {_tipoEmpleado}")
        };
    }
}


// ── DESPUÉS ───────────────────────────────────────────────────────────────────
// "abstract" significa que esta clase no se puede instanciar directamente.
// Solo existe para definir la estructura que las subclases deben implementar.
abstract class Empleado
{
    public string Nombre { get; }

    protected Empleado(string nombre)
    {
        Nombre = nombre;
    }

    // "abstract" obliga a cada subclase a dar su propia implementación
    public abstract int ObtenerBono();
    public abstract int ObtenerDiasVacaciones();

    // Factory Method: único lugar donde se decide qué subclase crear.
    // El resto del código usa Empleado.Crear(...) sin saber qué subclase es.
    public static Empleado Crear(string nombre, string tipoEmpleado)
    {
        return tipoEmpleado switch
        {
            "ingeniero" => new EmpleadoIngeniero(nombre),
            "vendedor" => new EmpleadoVendedor(nombre),
            "gerente" => new EmpleadoGerente(nombre),
            _ => throw new ArgumentException($"Tipo desconocido: {tipoEmpleado}")
        };
    }
}


// Cada subclase implementa SU comportamiento específico.
// No hay switch; el comportamiento queda encapsulado en la clase.
class EmpleadoIngeniero : Empleado
{
    public EmpleadoIngeniero(string nombre) : base(nombre) { }

    public override int ObtenerBono() => 5000;
    public override int ObtenerDiasVacaciones() => 20;
}

class EmpleadoVendedor : Empleado
{
    public EmpleadoVendedor(string nombre) : base(nombre) { }

    public override int ObtenerBono() => 3000;
    public override int ObtenerDiasVacaciones() => 15;
}

class EmpleadoGerente : Empleado
{
    public EmpleadoGerente(string nombre) : base(nombre) { }

    public override int ObtenerBono() => 8000;
    public override int ObtenerDiasVacaciones() => 25;
}
