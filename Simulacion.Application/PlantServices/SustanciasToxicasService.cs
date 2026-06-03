using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class SustanciasToxicasService
    {
        private readonly IDistribution _distribuciones;
        private double _pesoToxicoTotal = 0;
        private double _costoTotalDisposicion = 0;


        public SustanciasToxicasService(IDistribution distribucion)
        {
            _distribuciones = distribucion;
        }

        public ResultadoSustanciasToxicas ProcesarSustancias(double pesoTotalLote)
        {
            double pesoToxico = _distribuciones.GenerarBinomial(pesoTotalLote, 0.03); // 3$ del peso total es tóxico
            double costoDisposicion = pesoToxico * _distribuciones.GenerarUniforme(2.5, 5);

            _pesoToxicoTotal += pesoToxico;
            _costoTotalDisposicion += costoDisposicion;

            return new ResultadoSustanciasToxicas
            {
                PesoToxicoKg = Math.Round(pesoToxico,2),
                CostoDisposicion = Math.Round(costoDisposicion,2)
            };
        }

        public ResumenSustanciasToxicas ObtenerResumen () => new ResumenSustanciasToxicas
        {
            PesoToxicoTotalKg = Math.Round(_pesoToxicoTotal,2),
            CostoTotalDisposicion = Math.Round(_costoTotalDisposicion,2)
        };
    }
}
