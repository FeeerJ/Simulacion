using Simulacion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class SimulacionService
    {
        private readonly LlegadaService _llegada;
        private readonly SegmentacionService _segmentacion;
        private readonly PesoService _peso;
        public SimulacionService(LlegadaService llegada, SegmentacionService segmentacion, PesoService peso)
        {
            _llegada = llegada;
            _segmentacion = segmentacion;
            _peso = peso;
        }


        public ResultadoSimulacion Ejecutar(int dias = 30, int camionetasPorDia = 5)
        {
            var resumenPorDia = new List<ResumenDia>();

            for (int dia = 1; dia <= dias; dia++)
            {
                var resumenDia = new ResumenDia { Dia = dia };

                for (int camion = 0; camion < camionetasPorDia; camion++)
                {
                    var llegada = _llegada.ProcesarNuevaCamioneta();
                    var segmentacion = _segmentacion.ProcesoSegmentar(llegada.ParaDesmantelamiento);
                    var peso = _peso.CalcularPeso(segmentacion.TotalCRT, segmentacion.TotalLCD, segmentacion.TotalLED);

                    resumenDia.TotalDesmantelamiento += llegada.ParaDesmantelamiento;
                    resumenDia.TotalCRT += segmentacion.TotalCRT;
                    resumenDia.TotalLCD += segmentacion.TotalLCD;
                    resumenDia.TotalLED += segmentacion.TotalLED;
                    resumenDia.TotalRefurbishment += llegada.ParaRefurbishment;
                    resumenDia.TotalDescartados += llegada.Descartados;
                    resumenDia.PesoTotalKg += peso.PesoTotal;
                }

                resumenPorDia.Add(resumenDia);
            }

            return new ResultadoSimulacion
            {
                Dias = dias,
                CamionetasPorDia = camionetasPorDia,
                ResumenPorDia = resumenPorDia,
                Totales = new ResumenTotal
                {
                    TotalCRT = resumenPorDia.Sum(d => d.TotalCRT),
                    TotalLCD = resumenPorDia.Sum(d => d.TotalLCD),
                    TotalLED = resumenPorDia.Sum(d => d.TotalLED),
                    TotalRefurbishment = resumenPorDia.Sum(d => d.TotalRefurbishment),
                    TotalDescartados = resumenPorDia.Sum(d => d.TotalDescartados),
                    TotalDesmantelamiento = resumenPorDia.Sum(d => d.TotalDesmantelamiento),
                    TotalPeso = Math.Round(resumenPorDia.Sum(d => d.PesoTotalKg),2)
                }
            };
        }
    }
}
