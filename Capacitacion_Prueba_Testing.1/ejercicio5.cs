using System;

// EJEMPLO: Separar Consulta de Modificación


// ── ANTES ──
class AccesoAntes
{
    private string claveCorrecta = "1234";

    // Hace dos cosas: valida Y muestra mensaje
    public bool VerificarClave(string clave)
    {
        if (clave == claveCorrecta)
        {
            Console.WriteLine("Acceso permitido");
            return true;
        }
        else
        {
            Console.WriteLine("Acceso denegado");
            return false;
        }
    }
}


// ── DESPUÉS ──
class Acceso
{
    private string claveCorrecta = "1234";

    // Consulta: solo valida
    public bool EsClaveCorrecta(string clave)
    {
        return clave == claveCorrecta;
    }

    // Acción: solo muestra mensaje
    public void MostrarResultado(bool esCorrecta)
    {
        if (esCorrecta)
            Console.WriteLine("Acceso permitido");
        else
            Console.WriteLine("Acceso denegado");
    }
}