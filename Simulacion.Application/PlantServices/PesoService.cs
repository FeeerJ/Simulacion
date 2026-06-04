using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class PesoService
    {
        private readonly IDistribution _distribucion;
        private double _pesoTotalCRT = 0;
        private double _pesoTotalLCD = 0;
        private double _pesoTotalLED = 0;

        public PesoService(IDistribution distribucion, double pesoTotalCRT, double pesoTotalLCD, double pesoTotalLED)
        {
            _distribucion = distribucion;
            _pesoTotalCRT = pesoTotalCRT;
            _pesoTotalLCD = pesoTotalLCD;
            _pesoTotalLED = pesoTotalLED;
        }

        public PesoService(IDistribution distribuciones)
        {
            _distribucion = distribuciones;
        }

                // Calcula el peso total de los dispositivos basados en sus distribuciones estadisticas
        public ResultadoPeso CalcularPeso(int cantCRT, int cantLCD, int cantLED)
        {
            double pesoCRT = 0;
            double pesoLCD = 0;
            double pesoLED = 0;

            for (int i = 0; i < cantCRT; i++)
            {
                pesoCRT += _distribucion.GenerarUniforme(11, 104);
            }
            for (int i = 0; i < cantLCD; i++)
            {
                pesoLCD += _distribucion.GenerarUniforme(11, 49);
            }
            for (int i = 0; i < cantLED; i++)
            {
                pesoLED += _distribucion.GenerarUniforme(11, 39);
            }

            _pesoTotalCRT += pesoCRT;
            _pesoTotalLCD += pesoLCD;
            _pesoTotalLED += pesoLED;


            return new ResultadoPeso
            {
                PesoCRT = Math.Round(pesoCRT, 2),
                PesoLCD = Math.Round(pesoLCD, 2),
                PesoLED = Math.Round(pesoLED, 2),
                PesoTotal = Math.Round(pesoCRT + pesoLCD + pesoLED, 2)
            };
        }
        public ResumenPeso ObtenerResumen() => new ResumenPeso
        {
            PesoTotalCRT = Math.Round(_pesoTotalCRT, 2),
            PesoTotalLCD = Math.Round(_pesoTotalLCD, 2),
            PesoTotalLED = Math.Round(_pesoTotalLED, 2),
            PesoTotal = Math.Round(_pesoTotalCRT + _pesoTotalLCD + _pesoTotalLED, 2)
        };

    }
}

