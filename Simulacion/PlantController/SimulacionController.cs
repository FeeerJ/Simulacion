using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.PlantServices;

[ApiController]
[Route("api/simulacion")]
public class SimulacionController : ControllerBase
{
    private readonly SimulacionService _simulacion;

    public SimulacionController(SimulacionService simulacion)
    {
        _simulacion = simulacion;
    }

  
    // Endpoint principal para ejecutar la simulación de la planta.
    // Recibe los parámetros de configuración por query string y retorna los resultados de la simulación.
    [HttpPost("ejecutar")]
    public IActionResult Ejecutar(
        [FromQuery] int dias = 30, 
        [FromQuery] int camionetasPorDia = 5,
        [FromQuery] double capacidadAlmacenM3 = 375.0,
        [FromQuery] int operariosCRT = 2,
        [FromQuery] int operariosPlanas = 3)
    {
        // Validación básica de los parámetros de entrada.
        if (dias <= 0 || camionetasPorDia <= 0 || operariosCRT <= 0 || operariosPlanas <= 0)
            return BadRequest("Los parámetros deben ser mayores a 0.");

        // Se ejecuta la simulación con los parámetros recibidos.
        var resultado = _simulacion.Ejecutar(dias, camionetasPorDia, capacidadAlmacenM3, operariosCRT, operariosPlanas);
        return StatusCode(200, resultado);
    }
}
