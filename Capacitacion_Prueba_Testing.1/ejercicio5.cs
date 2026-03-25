// TÉCNICA 5: Separar Consulta de Modificación (Separate Query from Modifier)
//
// PROBLEMA:
//   Un método hace DOS cosas al mismo tiempo:
//     1. Devuelve un valor útil  → esto es una "Consulta" (Query)
//     2. Modifica el estado      → esto es un "Comando/Modificador"
//   Mezclarlo causa problemas: no se puede consultar sin causar efectos,
//   es difícil de probar y el comportamiento es impredecible.
//   Este principio se llama CQS: Separación Comando-Consulta.
//
// SOLUCIÓN:
//   Dividir en DOS métodos:
//     - Consulta:    solo devuelve datos, NO modifica nada (puede llamarse muchas veces)
//     - Modificador: solo cambia el estado, NO retorna nada útil (retorna void)
//
// POR QUÉ NO ES TRIVIAL:
//   Requiere identificar responsabilidades mezcladas, separar la lógica en dos
//   métodos con firmas distintas, y actualizar todos los lugares donde se usa.
// =============================================================================

// ── ANTES ────────────────────────────────────────────────────────────────────
class SistemaSeguridadAntes
{
    private readonly List<string> _intrusos = new() { "John Doe", "Jane Smith" };
    private bool _alertaEnviada = false;

    // PROBLEMA: Este método hace dos cosas a la vez:
    //   - Busca y RETORNA el intruso  (Consulta)
    //   - Envía alerta y cambia el estado (Modificador/efecto secundario)
    // No se puede preguntar "¿hay un intruso?" sin activar la alarma.
    public string VerificarIntrusoYAlertar(List<string> personasEnArea)
    {
        foreach (var persona in personasEnArea)
        {
            if (_intrusos.Contains(persona))
            {
                EnviarAlerta(persona);   // Efecto secundario: modifica estado
                _alertaEnviada = true;   // Efecto secundario: modifica estado
                return persona;          // Y además retorna un valor
            }
        }
        return null;
    }

    private void EnviarAlerta(string persona)
    {
        Console.WriteLine($"  [ANTES] ALERTA: Intruso detectado - {persona}");
    }
}

// ── DESPUÉS ───────────────────────────────────────────────────────────────────
class SistemaSeguridadDespues
{
    private readonly List<string> _intrusos = new() { "John Doe", "Jane Smith" };
    private bool _alertaEnviada = false;

    // CONSULTA: Solo lee y devuelve. No modifica absolutamente nada.
    // Se puede llamar muchas veces y el estado del sistema no cambia.
    // "string?" significa que puede devolver null si no hay intruso
    public string? BuscarIntruso(List<string> personasEnArea)
    {
        foreach (var persona in personasEnArea)
        {
            if (_intrusos.Contains(persona))
                return persona;
        }
        return null;
    }

    // MODIFICADOR/COMANDO: Solo modifica el estado. Retorna void (nada útil).
    // Quien llama este método decide cuándo ejecutarlo.
    public void EnviarAlerta(string persona)
    {
        Console.WriteLine($"  [DESPUÉS] ALERTA: Intruso detectado - {persona}");
        _alertaEnviada = true;
    }

    // Consulta adicional: permite verificar el estado sin efectos secundarios
    public bool AlertaFueEnviada() => _alertaEnviada;
}

// Uso del código refactorizado:
// El controlador maneja el flujo de forma explícita: primero consulta, luego actúa.
class ControladorSeguridad
{
    public void ManejarRevisionSeguridad(SistemaSeguridadDespues sistema, List<string> personas)
    {
        string? intruso = sistema.BuscarIntruso(personas);  // 1. Consulto
        if (intruso != null)
        {
            sistema.EnviarAlerta(intruso);                  // 2. Actúo solo si es necesario
        }
    }
}
