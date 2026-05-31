using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.PlantServices;

namespace Simulacion.PlantController
{
    [ApiController]
    [Route("api/segmentacion")]
    public class SegmentacionController : ControllerBase
    {
        private readonly SegmentacionService _segmentacion;

        public SegmentacionController(SegmentacionService segmentacion)
        {
            _segmentacion = segmentacion;
        }

        [HttpPost("segmentacion")]
        public IActionResult Segmentar([FromQuery] int cantidad)
        {

            if (cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor a 0.");
            }

            var resultado = _segmentacion.ProcesoSegmentar(cantidad);
            return Ok(resultado);
        }

        [HttpGet("Resumen")]
        public IActionResult ObtenerResumen()
        {
            var resumen = _segmentacion.ObtenerResumen();
            return Ok(resumen);
        }

    }
}
