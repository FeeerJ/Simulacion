using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.Dtos;
using Simulacion.Application.PlantServices;
using Simulacion.Application.Services;


namespace Simulacion.Controllers { 

    [ApiController]
    [Route("api/diagnosticos")]
public class DiagnosticsController : ControllerBase
    {
    [HttpGet("uniform")]
    public IActionResult TestUniform()
    {
        var numerosControlados = new List<double> { 0.5 };
        var servicio = new DistributionService(numerosControlados);

        double resultado = servicio.GenerarUniforme(10, 20);

        return Ok(new
        {
            Metodo = "Uniforme",
            InputU = 0.5,
            ValorEsperado = 15,
            ValorObtenido = resultado,
            Resultado = (resultado == 15) ? "EXITO" : "FALLO"
        });
    }

    [HttpGet("normal")]
    public IActionResult TestNormal()
    {
        //Box-Muller requiere dos variables U. Inyectamos 0.5 y 0.5.
        var numerosControlados = new List<double> { 0.5, 0.5 };
        var servicio = new DistributionService(numerosControlados);

        // Probamos con Media 100 y Desviación 10
        double resultado = servicio.GenerarNormal(100, 10);

        //Sabemos por calculadora que el resultado exacto con esos inputs es ~88.225
        double esperado = 88.225;
        double obtenidoRedondeado = Math.Round(resultado, 3);
        double margenError = 0.01;
        return Ok(new
        {
            Metodo = "Normal (Box-Muller)",
            InputsU = new[] { 0.5, 0.5 },
            Media = 100,
            Desviacion = 10,
            ValorEsperado = esperado,
            ValorObtenido = obtenidoRedondeado,
            Resultado = Math.Abs(obtenidoRedondeado - esperado) <= margenError ? "EXITO" : "FALLO"
        });
    }

    [HttpGet("tecnologia")]
    public IActionResult TestTecnologia([FromQuery] double valorUParam)
    {
        // Para probar la clasificacion de la tecnologia debes ingresar el valor de U manualmente, por ejemplo: 0,15- 0,14, etc. Deberia devolverte la tecnologia seleccionada
       
        var numerosControlados = new List<double> { valorUParam };
        var servicio = new DistributionService(numerosControlados);

        string tecnologiaResultante = servicio.DeterminarTecnologia();

        return Ok(new
        {
            Metodo = "Segmentación de Tecnología (Binomial/Empírica)",
            InputU_Recibido = valorUParam,
            ReglaAplicada = "CRT (<=0.15), LCD (<=0.65), LED (>0.65)",
            ClasificacionFinal = tecnologiaResultante
        });
    }

    [HttpGet("exponencial")]
    public IActionResult TestExponencial([FromQuery] double valorUParam, [FromQuery] double mediaMediaDias)
    {
            // Para probar este endpoint debemos ingresar manualmente un valor de U y la media de dias. 
        var numerosControlados = new List<double> { valorUParam };
        var servicio = new DistributionService(numerosControlados);

        double resultado = servicio.GenerarExponencial(mediaMediaDias);

        return Ok(new
        {
            Metodo = "Exponencial (Llegada Flete Internacional)",
            InputU = valorUParam,
            MediaDias = mediaMediaDias,
            DiasCalculados = Math.Round(resultado, 2)
        });
    }
}
}
