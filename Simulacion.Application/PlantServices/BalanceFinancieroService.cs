using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class BalanceFinancieroService
    {
        private readonly IDistribution _distribution;
        private double _pcbAcumuladoKg = 0;
        private double _pesoComunesAcumuladoKg = 0;
        private double _ingresosTotales = 0;
        private double _costosTotales = 0;
        public BalanceFinancieroService(IDistribution distribution)
        {
            _distribution = distribution;
        }

        public ResultadoBalanceDia ProcesarDia(
            int dia,
            int refurbishment,
            double pesoTotalKg,
            double cobreKg,
            double pcbKg,
            double costoToxicos)
        {
            double ingresosDia = 0;
            double costosDia = 0;
            bool huboDespacho = false;


            ingresosDia += refurbishment * 100;


            _pcbAcumuladoKg += pcbKg;


            double comunesDia = pesoTotalKg - cobreKg - pcbKg;
            _pesoComunesAcumuladoKg += comunesDia;


            costosDia += costoToxicos;


            if ((dia == 15 || dia == 30) && _pesoComunesAcumuladoKg > 0)
            {
                costosDia += 120;
                _pesoComunesAcumuladoKg = 0;
                huboDespacho = true;
            }


            if (dia == 30)
            {
                if (_pcbAcumuladoKg >= 50)
                    ingresosDia += 8500;
                else
                    costosDia += 350;
            }

            _ingresosTotales += ingresosDia;
            _costosTotales += costosDia;

            return new ResultadoBalanceDia
            {
                Dia = dia,
                IngresosDia = Math.Round(ingresosDia, 2),
                CostosDia = Math.Round(costosDia, 2),
                GananciaDia = Math.Round(ingresosDia - costosDia, 2),
                PCBacumuladoKg = Math.Round(_pcbAcumuladoKg, 4),
                IngresosTotalesAcumulados = Math.Round(_ingresosTotales, 2),
                CostosTotalesAcumulados = Math.Round(_costosTotales, 2),
                GananciaNetaAcumulada = Math.Round(_ingresosTotales - _costosTotales, 2),
                HuboDespachoFlete = huboDespacho
            };
        }
    }
}

