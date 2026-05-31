using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class SegmentacionService
    {
        public readonly IDistribution _distribuciones;
        private int _totalCRT = 0;
        private int _totalLED= 0;
        private int _totalLCD = 0;

        public SegmentacionService(IDistribution distribuciones)
        {
            _distribuciones = distribuciones;
        }

        public ResultadoSegmentacion ProcesoSegmentar(int cantidadParaDesmantelar)
        {
            int crt = 0;
            int lcd = 0;
            int led = 0;
            for (int i = 0; i <cantidadParaDesmantelar; i++)
            {
                double u = _distribuciones.GenerarUniforme(0, 1);
                if (u < 0.15)
                {
                    crt++;
                }
                else if (u < 0.65) // Entre 0.5 y 0.8 es LCD
                {
                    lcd++;
                }
                else // Entre 0.8 y 1 es LED
                {
                    led++;
                }
               
            }
            _totalCRT += crt;
            _totalLCD += lcd;
            _totalLED += led;


            return new ResultadoSegmentacion
            {
                TotalCRT = crt,
                TotalLCD = lcd,
                TotalLED = led,
                Total= crt+lcd+led
            };
        }


        public ResultadoSegmentacion ObtenerResumen()=> new ResultadoSegmentacion
        {
                TotalCRT = _totalCRT,
                TotalLCD = _totalLCD,
                TotalLED = _totalLED,
                Total = _totalCRT + _totalLCD + _totalLED
            };
    }
}
