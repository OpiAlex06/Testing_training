using System;
using System.Collections.Generic;

// =============================================================================
// TÉCNICA 3: Introducir Objeto Nulo (Null Object)
//
// PROBLEMA:
//   El código está lleno de "if (cliente == null)" en decenas de lugares.
//   Es fácil olvidar una verificación y provocar un NullReferenceException,
//   el error más común en C#.
//
// SOLUCIÓN:
//   Crear una clase "ClienteNulo" que implemente la misma interfaz que
//   Cliente pero con comportamiento neutro (descuento 0, correo vacío, etc.).
//   Así se elimina la necesidad de verificar null en todos lados.
//
// POR QUÉ ES NO TRIVIAL:
//   Requiere definir una interfaz (ICliente), crear la clase Objeto Nulo,
//   y cambiar la lógica de obtención de clientes en toda la aplicación.
// =============================================================================


// ── ANTES ────────────────────────────────────────────────────────────────────
class ClienteAntes
{
    public string Nombre { get; }
    public string Correo { get; }

    public ClienteAntes(string nombre, string correo)
    {
        Nombre = nombre;
        Correo = correo;
    }

    public double ObtenerDescuento() => 0.10;
}

class ProcesadorPedidosAntes
{
    // Verificaciones null repetidas en cada método que usa el cliente
    public double ProcesarPedido(ClienteAntes cliente)
    {
        double descuento;

        if (cliente == null)
        {
            Console.WriteLine("  Pedido de invitado - sin descuento");
            descuento = 0;
        }
        else
        {
            descuento = cliente.ObtenerDescuento();
        }

        if (cliente == null)
            Console.WriteLine("  Sin notificación por correo");
        else
            Console.WriteLine($"  Enviando correo a: {cliente.Correo}");

        return descuento;
    }
}


// ── DESPUÉS ───────────────────────────────────────────────────────────────────

// "interface" define un contrato: cualquier clase que la implemente
// DEBE tener estos métodos. En C# esto es la base del polimorfismo.
interface ICliente
{
    string Nombre { get; }
    string Correo { get; }
    double ObtenerDescuento();
    bool EsNulo { get; } // para identificar si es el objeto nulo si se necesita
}

// Cliente real: implementa la interfaz con datos reales
class Cliente : ICliente
{
    public string Nombre { get; }
    public string Correo { get; }
    public bool EsNulo => false;

    public Cliente(string nombre, string correo)
    {
        Nombre = nombre;
        Correo = correo;
    }

    public double ObtenerDescuento() => 0.10;
}

// Objeto Nulo: misma interfaz, comportamiento vacío/neutro.
// Representa "la ausencia de cliente" de forma segura.
class ClienteNulo : ICliente
{
    public string Nombre => "Invitado";
    public string Correo => string.Empty;  // string.Empty == "" en C#
    public bool EsNulo => true;

    public double ObtenerDescuento() => 0;  // Sin descuento para clientes anónimos
}

// Simulación de una base de datos de clientes
class RepositorioClientes
{
    private static readonly Dictionary<int, ICliente> _db = new()
    {
        { 1, new Cliente("Ana García", "ana@example.com") },
        { 2, new Cliente("Carlos Ruiz", "carlos@example.com") }
    };

    // Devuelve ClienteNulo en lugar de null. ¡Nunca retorna null!
    public ICliente ObtenerPorId(int id)
    {
        return _db.TryGetValue(id, out var cliente)
            ? cliente
            : new ClienteNulo();
    }
}

class ProcesadorPedidos
{
    private readonly RepositorioClientes _repo = new();

    public double ProcesarPedido(int clienteId)
    {
        ICliente cliente = _repo.ObtenerPorId(clienteId);

        // ¡Sin ningún "if null"! Se llama igual sin importar el tipo real
        double descuento = cliente.ObtenerDescuento();

        if (!string.IsNullOrEmpty(cliente.Correo))
            Console.WriteLine($"  Enviando correo a: {cliente.Correo}");
        else
            Console.WriteLine($"  Sin correo para: {cliente.Nombre}");

        return descuento;
    }
}

