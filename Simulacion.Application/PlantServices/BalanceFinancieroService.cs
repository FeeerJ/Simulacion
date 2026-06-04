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

        // Procesa el balance financiero diario (ingresos y costos) de la planta
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


            // Ingresos por equipos que aplican a refurbishment
            ingresosDia += refurbishment * 100;

            // Acumular kilogramos de PCB
            _pcbAcumuladoKg += pcbKg;

            // Los materiales comunes se calculan restando el cobre y el pcb
            double comunesDia = pesoTotalKg - cobreKg - pcbKg;
            _pesoComunesAcumuladoKg += comunesDia;

            // Sumar los costos de disposición de las sustancias tóxicas de este día
            costosDia += costoToxicos;

            // Despacho quincenal de materiales comunes
            if ((dia == 15 || dia == 30) && _pesoComunesAcumuladoKg > 0)
            {
                costosDia += 120; // Costo de flete
                _pesoComunesAcumuladoKg = 0;
                huboDespacho = true;
            }

            // Liquidación mensual del lote de PCB
            if (dia == 30)
            {
                if (_pcbAcumuladoKg >= 50)
                    ingresosDia += 8500; // Ingreso por venta de lote
                else
                    costosDia += 350;    // Costo si no se alcanza el peso mínimo
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

