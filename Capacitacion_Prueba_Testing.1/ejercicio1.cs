using System;

// =============================================================================
// TÉCNICA 1: Reemplazar Método con Objeto Método (Replace Method with Method Object)
//
// PROBLEMA:
//   Un método tiene lógica muy larga con muchas variables locales (base,
//   descuento, bono, etc.) que dependen unas de otras. Si intentamos dividirlo
//   en métodos más pequeños con "Extraer Método", tendríamos que pasar TODAS
//   esas variables como parámetros, lo que es igual de confuso.
//
// SOLUCIÓN:
//   Convertir ese método en una CLASE propia. Así las variables locales se
//   convierten en campos (fields) de la clase y todos los métodos nuevos
//   pueden accederlos sin necesidad de pasarlos como parámetros.
//
// POR QUÉ ES NO TRIVIAL:
//   No es un simple renombrado. Requiere crear una nueva clase, mover lógica
//   de negocio a ella y cambiar cómo se hace el cálculo en el código original.
// =============================================================================

// ── ANTES ────────────────────────────────────────────────────────────────────
class PedidoAntes
{
    public int Cantidad { get; set; }
    public double PrecioPorArticulo { get; set; }
    public double PrecioBaseSecundario { get; set; }

    public PedidoAntes(int cantidad, double precioPorArticulo, double precioBaseSecundario)
    {
        Cantidad = cantidad;
        PrecioPorArticulo = precioPorArticulo;
        PrecioBaseSecundario = precioBaseSecundario;
    }

    // Método largo con variables locales interdependientes.
    // Muy difícil de dividir sin pasar muchos parámetros entre sub-métodos.
    public double CalcularPrecio()
    {
        double precioBase = Cantidad * PrecioPorArticulo;
        double baseSecundaria = PrecioBaseSecundario * 0.1;

        double valorImportante1;
        if (precioBase > 1000)
            valorImportante1 = precioBase * 0.95;
        else
            valorImportante1 = precioBase * 0.98;

        double valorImportante2 = valorImportante1 + baseSecundaria;
        double valorImportante3 = valorImportante2 > 500
            ? valorImportante2 * 0.9
            : valorImportante2;

        return valorImportante3 - (valorImportante3 * 0.05);
    }
}

// ── DESPUÉS ───────────────────────────────────────────────────────────────────
// Esta clase nueva reemplaza al método largo. Sus campos son las antiguas
// variables locales; ahora todos los sub-métodos pueden acceder a ellos.
class CalculadorDePrecio
{
    // Datos originales del pedido
    private readonly int _cantidad;
    private readonly double _precioPorArticulo;
    private readonly double _precioBaseSecundario;

    // Las variables locales del método original se convierten en campos
    private double _precioBase;
    private double _baseSecundaria;
    private double _valorImportante1;
    private double _valorImportante2;
    private double _valorImportante3;

    // El constructor recibe el Pedido y extrae lo que necesita
    public CalculadorDePrecio(PedidoDespues pedido)
    {
        _cantidad = pedido.Cantidad;
        _precioPorArticulo = pedido.PrecioPorArticulo;
        _precioBaseSecundario = pedido.PrecioBaseSecundario;
    }

    // Método principal: orquesta los pasos del cálculo
    public double Calcular()
    {
        CalcularBase();
        CalcularDescuentos();
        AplicarBono();
        return PrecioFinal();
    }

    // Cada sub-método hace UNA sola cosa y accede a los campos libremente
    private void CalcularBase()
    {
        _precioBase = _cantidad * _precioPorArticulo;
        _baseSecundaria = _precioBaseSecundario * 0.1;
    }

    private void CalcularDescuentos()
    {
        _valorImportante1 = _precioBase > 1000
            ? _precioBase * 0.95
            : _precioBase * 0.98;

        _valorImportante2 = _valorImportante1 + _baseSecundaria;
    }

    private void AplicarBono()
    {
        _valorImportante3 = _valorImportante2 > 500
            ? _valorImportante2 * 0.9
            : _valorImportante2;
    }

    private double PrecioFinal()
    {
        return _valorImportante3 - (_valorImportante3 * 0.05);
    }
}

class PedidoDespues
{
    public int Cantidad { get; set; }
    public double PrecioPorArticulo { get; set; }
    public double PrecioBaseSecundario { get; set; }

    public PedidoDespues(int cantidad, double precioPorArticulo, double precioBaseSecundario)
    {
        Cantidad = cantidad;
        PrecioPorArticulo = precioPorArticulo;
        PrecioBaseSecundario = precioBaseSecundario;
    }

    // Ahora el método es una sola línea: delega en CalculadorDePrecio
    public double CalcularPrecio()
    {
        return new CalculadorDePrecio(this).Calcular();
    }
}

