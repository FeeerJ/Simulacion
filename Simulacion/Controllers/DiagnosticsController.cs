using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacion.Application.Dtos;
using Simulacion.Application.PlantServices;
using Simulacion.Application.Services;


namespace Simulacion.Controllers { 
    /*
    [ApiController]
    [Route("api/diagnosticos")]
public class DiagnosticsController : ControllerBase
    {
        private readonly CongruentialMethodService _generador;

        public DiagnosticsController (CongruentialMethodService generador)
        {
            _generador = generador;
        }

        private IEnumerable<double> GenerarU(long semilla, int total)
        {
            var config = new MixedCongruentialModel(
                M: 100000,
                N0: semilla,
                A:1664525,
                C: 1013904223,
                TotalNumeros: total
                );
            return _generador.GenerateMixed(config.M, config.N0, config.A, config.C, config.TotalNumeros);
        }


        [HttpGet("binomial")]
        public IActionResult TestBinomial([FromQuery] long semilla = 1234, [FromQuery] double n = 100, [FromQuery] double p = 0.9)
        {
           
            var listaU = GenerarU(semilla, (int)n + 10).ToList(); // Pedimos n números al metodo CongruencialMixto para la Binomial
            var servicio = new DistributionService(listaU);
            double resultados = servicio.GenerarBinomial(n, p);

            return Ok(new
            {
                Metodo = "Binomial",
                SemillaUsada = semilla,
                U_GeneradosPorMidSquare = listaU.Take((int)n).ToList(),
                Parametros = $"n={n}, p={p}",
                ExitosObtenidos = resultados,
                PorcentajeReal = (resultados / n) * 100 + "%",
                NumerosSobrantes = listaU.Count() - (int)n
            });

        }
       
        [HttpGet("uniform")]
        public IActionResult TestUniform([FromQuery] long semilla = 8453)
        {
            // 1. Usamos CongruencialMixto para pedir 1 solo número (k=4 dígitos)
            var listaU = GenerarU(semilla, 1);
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
            // Box-Muller requiere DOS variables U. Le pedimos 2 a CongruencialMixto.
            var listaU = GenerarU(semilla, 2);
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
            // Pedimos 1 número a CongruencialMixto
            var listaU = GenerarU(semilla, 1);
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
            // Pedimos 1 número a CongruencialMixto
            var listaU = GenerarU(semilla,1);
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
    }*/
}
