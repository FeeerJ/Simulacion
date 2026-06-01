using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.Services;
using Simulacion.Application.Dtos;



namespace Simulacion.Controllers
{/*
   // [Authorize]
    [ApiController]
    [Route("api/simuladores")]
    public class MidSqueareController : ControllerBase
    {
        private readonly MidSquareService _midSquareservice;

        public MidSqueareController(MidSquareService service)
        {
            _midSquareservice = service;
        }


        [HttpPost("Cuadrados Medios")]
        public IActionResult GetCuadrados([FromBody] MidSquareRequestDto request)
        {
            var result = _midSquareservice.Generar(request.M, request.N, request.TOT);
            return Ok(result);
        }

        
    }*/
}
