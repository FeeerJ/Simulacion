using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class LlegadaService
    {
        private readonly IDistribution _distribuciones;
        private int _totalDispositivos = 0;
        private int _totalCamionetas = 0;
        private int _totalResiduosInteres = 0;
        private int _totalAptos = 0;
        private int _totalDescartados = 0;
        private int _totalRefurbishment = 0;
        private int _totalDesmantelamiento = 0;
        public LlegadaService(IDistribution distribuciones)
        {
            _distribuciones = distribuciones;
        }

        public ResultadoCamioneta ProcesarNuevaCamioneta()
        {
            int cantDispositivos = (int)_distribuciones.GenerarNormal(80, 15);
            int cantTVMonitores = (int)_distribuciones.GenerarBinomial(cantDispositivos, 0.20);

            
            int cantAptos = (int)_distribuciones.GenerarBinomial(cantTVMonitores, 0.9);
            int cantDescartados = cantTVMonitores - cantAptos;

            
            int cantRefurbishment = 0;
            int cantDesmantelamiento = cantAptos;

            
            _totalCamionetas++;
            _totalDispositivos += cantTVMonitores;
            _totalResiduosInteres += cantTVMonitores;
            _totalAptos += cantAptos;
            _totalDescartados += cantDescartados;


            _totalRefurbishment += 0;
            _totalDesmantelamiento += cantAptos;


            return new ResultadoCamioneta
            {
                DispositivosTotales = cantDispositivos,
                TVMonitoresFiltrados = cantTVMonitores,
                Aptos = cantAptos,
                Descartados = cantDescartados,
                ParaRefurbishment = cantRefurbishment,
                ParaDesmantelamiento = cantDesmantelamiento
            };
        }

        public ResumenLlegada ObtenerResumen()
        {
            return new ResumenLlegada
            {
                TotalCamiones = _totalCamionetas,
                TotalDispositivosBrutos = _totalDispositivos,
                TotalTVMonitores = _totalResiduosInteres,
                TotalAptos = _totalAptos,
                TotalDescartados = _totalDescartados,
                TotalRefurbishment = _totalRefurbishment,
                TotalDesmantelamiento = _totalDesmantelamiento
            };

        }


    }



}

