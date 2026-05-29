using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.PlantServices;

namespace Simulacion.PlantController
{
    [ApiController]
    [Route("api/llegadaResiduos")]
    public class LlegadaResiduosController : ControllerBase

    {
        private readonly LlegadaService _llegada;

        public LlegadaResiduosController(LlegadaService llegada)
        {
            _llegada = llegada;
        }

        [HttpPost("Procesar")]
        public IActionResult ProcesarCaminoeta()
        {
            var camioneta = _llegada.ProcesarNuevaCamioneta();
            return Ok(camioneta);
        }
    }


}
