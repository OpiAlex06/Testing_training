using System;

// =============================================================================
// EJEMPLO CORTO: Reemplazar Código de Tipo con Subclases
// =============================================================================


// ── ANTES ────────────────────────────────────────────────────────────────────
class EmpleadoAntes
{
    public string Tipo { get; set; }

    public int ObtenerBono()
    {
        if (Tipo == "ingeniero")
            return 5000;
        else if (Tipo == "gerente")
            return 8000;
        else
            return 0;
    }
}


// ── DESPUÉS ───────────────────────────────────────────────────────────────────
abstract class Empleado
{
    public abstract int ObtenerBono();

    public static Empleado Crear(string tipo)
    {
        if (tipo == "ingeniero")
            return new Ingeniero();
        else if (tipo == "gerente")
            return new Gerente();
        else
            throw new Exception("Tipo no válido");
    }
}


class Ingeniero : Empleado
{
    public override int ObtenerBono()
    {
        return 5000;
    }
}

class Gerente : Empleado
{
    public override int ObtenerBono()
    {
        return 8000;
    }
}