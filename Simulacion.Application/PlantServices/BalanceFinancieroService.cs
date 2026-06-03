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
        private int _proximoDiaFlete;

        public BalanceFinancieroService(IDistribution distribution)
        {
            _distribution = distribution;
            // Inicializa esta variable para que el primer flete ocurra sumando al día 0
            _proximoDiaFlete = (int)Math.Round(_distribution.GenerarExponencial(15));
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

            // ingreso por refurbishment
            ingresosDia += refurbishment * 100;

            // acumula PCB
            _pcbAcumuladoKg += pcbKg;

            // materiales comunes del día 
            double comunesDia = pesoTotalKg - cobreKg - pcbKg;
            _pesoComunesAcumuladoKg += comunesDia;

            // costo por sustancias tóxicas
            costosDia += costoToxicos;

            // Flete genérico usando el tiempo dinámico y distribución exponencial
            if (dia >= _proximoDiaFlete && _pesoComunesAcumuladoKg > 0)
            {
                // Suma el costo logístico
                costosDia += 120;
                
                // Se despacha el stock, reinicia el acumulador
                _pesoComunesAcumuladoKg = 0; 
                huboDespacho = true;

                // Programa el próximo flete
                _proximoDiaFlete = dia + (int)Math.Round(_distribution.GenerarExponencial(15));
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
                GananciaNetaAcumulada = Math.Round(_ingresosTotales - _costosTotales, 2),
                HuboDespachoFlete = huboDespacho
            };
        }
    }
}
