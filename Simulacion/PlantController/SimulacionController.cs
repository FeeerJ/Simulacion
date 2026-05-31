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

    // POST api/simulacion/ejecutar?dias=30&camionetasPorDia=5
    [HttpPost("ejecutar")]
    public IActionResult Ejecutar([FromQuery] int dias = 30, [FromQuery] int camionetasPorDia = 5)
    {
        if (dias <= 0 || camionetasPorDia <= 0)
            return BadRequest("Los parámetros deben ser mayores a 0.");

        var resultado = _simulacion.Ejecutar(dias, camionetasPorDia);
        return StatusCode(200, resultado);
    }
}