using System;

// EJEMPLO CORTO: Reemplazar Método con Objeto Método (Replace Method with Method Object)


// ── ANTES ──
class PedidoAntes
{
    public int Cantidad { get; set; }
    public double Precio { get; set; }

    public double CalcularTotal()
    {
        double total = Cantidad * Precio;

        // Descuento si es mayor a 100
        if (total > 100)
            total = total * 0.9;

        return total;
    }
}


// ── DESPUÉS ──
class Calculador
{
    private int cantidad;
    private double precio;

    public Calculador(PedidoDespues pedido)
    {
        cantidad = pedido.Cantidad;
        precio = pedido.Precio;
    }

    public double Calcular()
    {
        double total = cantidad * precio;

        if (total > 100)
            total *= 0.9;

        return total;
    }
}

class PedidoDespues
{
    public int Cantidad { get; set; }
    public double Precio { get; set; }

    public double CalcularTotal()
    {
        return new Calculador(this).Calcular();
    }
}