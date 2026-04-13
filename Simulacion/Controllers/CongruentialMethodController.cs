using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.Dtos;
using Simulacion.Application.Services;

namespace Simulacion.Controllers;

[ApiController]
[Route("api/simuladores")]

public class CongruentialMethodController : ControllerBase
{
    private readonly CongruentialMethodService _congruentialService;

    public CongruentialMethodController(CongruentialMethodService congruentialService)
    {
        this._congruentialService = congruentialService;
    }

    [HttpPost("additiveCongruential")]

    public IActionResult GetAdditiveCongruential([FromBody] AdditiveCongruentialModel request)
    {
        var result = _congruentialService.GenerateAdditive(request.M, request.N0, request.N1, request.TotalNumeros);
        return Ok(result);
    }

    [HttpPost("multiplicativeCongruential")]
    public IActionResult GetMultiplicativeCongruential([FromBody] MultiplicativeCongruentialModel request)
    {
        var result = _congruentialService.GenerateMultiplicative(request.M, request.N0, request.A, request.TotalNumeros);
        return Ok(result);
    }

    [HttpPost("mixedCongruential")]
    public IActionResult GetMixedCongruential([FromBody] MixedCongruentialModel request)
    {
        var result = _congruentialService.GenerateMixed(request.M, request.N0, request.A, request.C, request.TotalNumeros);
        return Ok(result);
    }
}
