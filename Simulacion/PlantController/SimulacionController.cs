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

  
    [HttpPost("ejecutar")]
    public IActionResult Ejecutar(
        [FromQuery] int dias = 30, 
        [FromQuery] int camionetasPorDia = 5,
        [FromQuery] double capacidadAlmacenM3 = 375.0,
        [FromQuery] int operariosCRT = 2,
        [FromQuery] int operariosPlanas = 3)
    {
        if (dias <= 0 || camionetasPorDia <= 0 || operariosCRT <= 0 || operariosPlanas <= 0)
            return BadRequest("Los parámetros deben ser mayores a 0.");

        var resultado = _simulacion.Ejecutar(dias, camionetasPorDia, capacidadAlmacenM3, operariosCRT, operariosPlanas);
        return StatusCode(200, resultado);
    }
}