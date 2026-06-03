using Simulacion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class BalanceFinancieroService
    {
        private double _pcbAcumuladoKg = 0;
        private double _pesoComunesAcumuladoKg = 0;
        private double _ingresosTotales = 0;
        private double _costosTotales = 0;

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

            // ingreso por refurbishment
            ingresosDia += refurbishment * 100;

            // acumula PCB
            _pcbAcumuladoKg += pcbKg;

            // mteriales comunes del día 
            double comunesDia = pesoTotalKg - cobreKg - pcbKg;
            _pesoComunesAcumuladoKg += comunesDia;

            // costo por sustancias tóxicas
            costosDia += costoToxicos;

            // flete genérico cada 15 días si hay stock de mat comun
            if ((dia == 15 || dia == 30) && _pesoComunesAcumuladoKg > 0)
            {
                costosDia += 120;
                _pesoComunesAcumuladoKg = 0; // Se despacha el stock
            }

            // Al día 30 evaluar PCB
            if (dia == 30)
            {
                if (_pcbAcumuladoKg >= 50)
                    ingresosDia += 8500;
                else
                    costosDia += 350; // costo de mantener
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
                GananciaNetaAcumulada = Math.Round(_ingresosTotales - _costosTotales, 2)
            };
        }
    }
}
