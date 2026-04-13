using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.Services;
using Simulacion.Application.Dtos;



namespace Simulacion.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/simuladores")]
    public class LehmerController : ControllerBase
    {
        private readonly LehmerService _lehmerService;

        public LehmerController(LehmerService service)
        {
            _lehmerService = service;
        }


        [HttpPost("Lehmer")]
        public IActionResult GetLehmer([FromBody] LehmerRequestDto request)
        {
            var result = _lehmerService.GenerarLehmerDesplazamiento(
                request.n0,
                request.t,
                request.k,
                request.n);
            return Ok(result);
        }


    }
}
