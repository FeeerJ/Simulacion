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
        private int _totalLED = 0;
        private int _totalLCD = 0;
        private int _totalRefurbishment = 0;

        public SegmentacionService(IDistribution distribuciones)
        {
            _distribuciones = distribuciones;
        }

        public ResultadoSegmentacion ProcesoSegmentar(int aptos)
        {
            int crt = 0;
            int lcd = 0;
            int led = 0;

            // 1. Segmentar TODOS los aptos
            for (int i = 0; i < aptos; i++)
            {
                double u = _distribuciones.GenerarUniforme(0, 1);
                if (u < 0.15)
                {
                    crt++;
                }
                else if (u < 0.65) // Entre 0.15 y 0.65 es LCD (50%)
                {
                    lcd++;
                }
                else // Entre 0.65 y 1 es LED (35%)
                {
                    led++;
                }
            }

            // 2. Calcular Refurbishment (15% solo de LCD y LED)
            int lcdRefurbished = (int)_distribuciones.GenerarBinomial(lcd, 0.15);
            int ledRefurbished = (int)_distribuciones.GenerarBinomial(led, 0.15);
            int refurbishment = lcdRefurbished + ledRefurbished;

            // 3. Lo que realmente va a desmantelamiento
            int lcdDesmantelamiento = lcd - lcdRefurbished;
            int ledDesmantelamiento = led - ledRefurbished;

            _totalCRT += crt;
            _totalLCD += lcdDesmantelamiento;
            _totalLED += ledDesmantelamiento;
            _totalRefurbishment += refurbishment;

            return new ResultadoSegmentacion
            {
                TotalCRT = crt, // CRT siempre va 100% a desmantelamiento
                TotalLCD = lcdDesmantelamiento,
                TotalLED = ledDesmantelamiento,
                TotalRefurbishment = refurbishment,
                Total = crt + lcdDesmantelamiento + ledDesmantelamiento
            };
        }


        public ResultadoSegmentacion ObtenerResumen() => new ResultadoSegmentacion
        {
            TotalCRT = _totalCRT,
            TotalLCD = _totalLCD,
            TotalLED = _totalLED,
            TotalRefurbishment = _totalRefurbishment,
            Total = _totalCRT + _totalLCD + _totalLED
        };
    }
}
