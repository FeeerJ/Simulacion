using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.Dtos;
using Simulacion.Application.Services;

namespace Simulacion.Controllers;

[ApiController]
[Route("api/simuladores")]

public class AdditiveCongruentialController : ControllerBase
{
    private readonly AdditiveCongruentialService _additiveCongruentialService;

    public AdditiveCongruentialController(AdditiveCongruentialService additiveCongruentialService)
    {
        this._additiveCongruentialService = additiveCongruentialService;
    }

    [HttpPost("additiveCongruential")]

    public IActionResult GetAdditiveCongruential([FromBody] AdditiveCongruentialModel request)
    {
        var result = _additiveCongruentialService.Generar(request.M, request.N0, request.N1, request.TotalNumeros);
        return Ok(result);
    }
}
