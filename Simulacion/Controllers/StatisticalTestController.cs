using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.Dtos;
using Simulacion.Application.Services;

namespace Simulacion.Controllers { 
/*
[ApiController]
[Route("api/pruebasEstadisticas")]
public class StatisticalTestController : ControllerBase
{
    private readonly StatisticalTestsService _pruebasService;

    public StatisticalTestController(StatisticalTestsService pruebasService)
    {
        this._pruebasService = pruebasService;
    }

    [HttpPost("promedios")]
    public IActionResult GetPruebaPromedios([FromBody] PruebasEstadisticasRequesDto.PromediosRequestDto request)
    {
        var result = _pruebasService.PruebaPromedios(request.Numeros, request.ZAlfa);
        return Ok(result);
    }

    [HttpPost("frecuencia")]
    public IActionResult GetPruebaFrecuencia([FromBody] PruebasEstadisticasRequesDto.FrecuenciaRequestDto request)
    {
        var result = _pruebasService.PruebaFrecuencia(request.Numeros, request.CantidadSubintervalos, request.ChiCuadradoAlfa);
        return Ok(result);
    }

    [HttpPost("serie")]
    public IActionResult GetPruebaSerie([FromBody] PruebasEstadisticasRequesDto.SerieRequestDto request)
    {
        var result = _pruebasService.PruebaSerie(request.Numeros, request.X, request.ChiCuadradoAlfa);
        return Ok(result);
    }

    [HttpPost("kolmogorov-smirnov")]
    public IActionResult GetPruebaKS([FromBody] PruebasEstadisticasRequesDto.KSRequestDto request)
    {
        var result = _pruebasService.PruebaKS(request.Numeros, request.DAlfaN);
        return Ok(result);
    }

    [HttpPost("corridas")]
    public IActionResult GetPruebaCorridas([FromBody] PruebasEstadisticasRequesDto.CorridasRequestDto request)
    {
        var result = _pruebasService.PruebaCorridasMedia(request.Numeros, request.ChiCuadradoAlfa);
        return Ok(result);
    }*/
}
