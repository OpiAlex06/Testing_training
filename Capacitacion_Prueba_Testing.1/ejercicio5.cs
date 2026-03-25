using System;

// EJEMPLO CORTO: Separar Consulta de Modificación ( Separate Query from Modifier (CQS))

// ── ANTES ──
class SeguridadAntes
{
    private List<string> intrusos = new() { "Juan" };

    // Hace dos cosas: busca Y muestra alerta
    public string Verificar(List<string> personas)
    {
        foreach (var p in personas)
        {
            if (intrusos.Contains(p))
            {
                Console.WriteLine("Alerta!");
                return p;
            }
        }
        return null;
    }
}


// ── DESPUÉS ──
class Seguridad
{
    private List<string> intrusos = new() { "Juan" };

    // Consulta: solo busca
    public string Buscar(List<string> personas)
    {
        foreach (var p in personas)
        {
            if (intrusos.Contains(p))
                return p;
        }
        return null;
    }

    // Modificador: solo hace la acción
    public void Alertar(string persona)
    {
        Console.WriteLine("Alerta!");
    }
}