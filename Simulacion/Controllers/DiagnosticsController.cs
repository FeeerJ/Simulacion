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
        private readonly MidSquareService _generador;

        public DiagnosticsController (MidSquareService generador)
        {
            _generador = generador;
        }

        [HttpGet("uniform")]
        public IActionResult TestUniform([FromQuery] long semilla = 8453)
        {
            // 1. Usamos MidSquare para pedir 1 solo número (k=4 dígitos)
            var listaU = _generador.Generar(semilla, 4, 1);
            var servicio = new DistributionService(listaU);

            double resultado = servicio.GenerarUniforme(10, 20);

            return Ok(new
            {
                Metodo = "Uniforme",
                SemillaUsada = semilla,
                U_GeneradoPorMidSquare = listaU.First(),
                Rango = "10 a 20",
                ValorObtenido = Math.Round(resultado, 2)
            });
        }

        [HttpGet("normal")]
        public IActionResult TestNormal([FromQuery] long semilla = 8453)
        {
            // Box-Muller requiere DOS variables U. Le pedimos 2 a MidSquare.
            var listaU = _generador.Generar(semilla, 4, 2);
            var servicio = new DistributionService(listaU);

            double resultado = servicio.GenerarNormal(100, 10);

            return Ok(new
            {
                Metodo = "Normal (Box-Muller)",
                SemillaUsada = semilla,
                InputsU_Generados = listaU.ToList(),
                Media = 100,
                Desviacion = 10,
                ValorObtenido = Math.Round(resultado, 3)
            });
        }

        [HttpGet("tecnologia")]
        public IActionResult TestTecnologia([FromQuery] long semilla = 8453)
        {
            // Pedimos 1 número a MidSquare
            var listaU = _generador.Generar(semilla, 4, 1);
            var servicio = new DistributionService(listaU);

            string tecnologiaResultante = servicio.DeterminarTecnologia();

            return Ok(new
            {
                Metodo = "Segmentación de Tecnología (Binomial/Empírica)",
                SemillaUsada = semilla,
                U_GeneradoPorMidSquare = listaU.First(),
                ReglaAplicada = "CRT (<=0.15), LCD (<=0.65), LED (>0.65)",
                ClasificacionFinal = tecnologiaResultante
            });
        }

        [HttpGet("exponencial")]
        public IActionResult TestExponencial([FromQuery] double mediaDias = 15, [FromQuery] long semilla = 8453)
        {
            // Pedimos 1 número a MidSquare
            var listaU = _generador.Generar(semilla, 4, 1);
            var servicio = new DistributionService(listaU);

            double resultado = servicio.GenerarExponencial(mediaDias);

            return Ok(new
            {
                Metodo = "Exponencial (Llegada Flete Internacional)",
                SemillaUsada = semilla,
                U_GeneradoPorMidSquare = listaU.First(),
                MediaDias = mediaDias,
                DiasCalculados = Math.Round(resultado, 2)
            });
        }
    }
}
