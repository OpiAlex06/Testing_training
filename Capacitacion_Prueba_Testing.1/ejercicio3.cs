using System;

// EJEMPLO: Objeto Nulo (Introduce Null Object) 


// ── ANTES ──
class ClienteAntes
{
    public string Correo { get; set; }

    public double ObtenerDescuento()
    {
        return 0.1;
    }
}

class PedidoAntes
{
    public void Procesar(ClienteAntes cliente)
    {
        double descuento;

        if (cliente == null)
            descuento = 0;
        else
            descuento = cliente.ObtenerDescuento();

        if (cliente == null)
            Console.WriteLine("Sin correo");
        else
            Console.WriteLine(cliente.Correo);
    }
}


// ── DESPUÉS ──
interface ICliente
{
    string Correo { get; }
    double ObtenerDescuento();
}

class Cliente : ICliente
{
    public string Correo { get; set; }

    public double ObtenerDescuento()
    {
        return 0.1;
    }
}

// Objeto nulo: evita usar "if (cliente == null)"
class ClienteNulo : ICliente
{
    public string Correo => "";

    public double ObtenerDescuento()
    {
        return 0;
    }
}

class Pedido
{
    public void Procesar(ICliente cliente)
    {
        double descuento = cliente.ObtenerDescuento();

        if (cliente.Correo == "")
            Console.WriteLine("Sin correo");
        else
            Console.WriteLine(cliente.Correo);
    }
}