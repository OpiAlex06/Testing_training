using System;

// EJEMPLO CORTO: Reemplazar Condicional con Polimorfismo

// ── ANTES ──
class PajaroAntes
{
    public string Tipo { get; set; }

    public double ObtenerVelocidad()
    {
        if (Tipo == "europeo")
            return 12;
        else if (Tipo == "africano")
            return 10;
        else
            return 0;
    }
}


// ── DESPUÉS ──
abstract class Pajaro
{
    public abstract double ObtenerVelocidad();
}

class PajaroEuropeo : Pajaro
{
    public override double ObtenerVelocidad()
    {
        return 12;
    }
}

class PajaroAfricano : Pajaro
{
    public override double ObtenerVelocidad()
    {
        return 10;
    }
}