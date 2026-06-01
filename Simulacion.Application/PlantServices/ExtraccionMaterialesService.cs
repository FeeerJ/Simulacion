using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class ExtraccionMaterialesService
    {
        private readonly IDistribution _distribuciones;

        // Acumuladores globales
        private double _cobreTotalG = 0;
        private double _oroTotalG = 0;
        private double _plataTotalG = 0;
        private double _pesoTotalPCBKg = 0;

        public ExtraccionMaterialesService(IDistribution distribuciones)
        {
            _distribuciones = distribuciones;
        }

        public ResultadoExtraccion ExtraerMateriales(int cantCRT, int cantLCD, int cantLED)
        {
            var extraccionesCRT = new List<ExtraccionUnidad>();
            var extraccionesLCD = new List<ExtraccionUnidad>();
            var extraccionesLED = new List<ExtraccionUnidad>();

            double cobreLote = 0, oroLote = 0, plataLote = 0, pcbLote = 0;

            // CRT extracción de cobre por unidad
            for (int i = 0; i < cantCRT; i++)
            {
                double cobre = _distribuciones.GenerarNormal(450, 50)/1000;
                cobreLote += cobre;
                extraccionesCRT.Add(new ExtraccionUnidad { CobreG = Math.Round(cobre, 2) });
            }

            // LCD extracción de oro y plata por unidad PCB
            for (int i = 0; i < cantLCD; i++)
            {
                double pcbKg = _distribuciones.GenerarUniforme(0.1, 0.2);
                double oro = pcbKg * _distribuciones.GenerarNormal(0.15, 0.05);
                double plata = pcbKg * _distribuciones.GenerarNormal(0.30, 0.05);
                oroLote += oro; plataLote += plata; pcbLote += pcbKg;
                extraccionesLCD.Add(new ExtraccionUnidad
                {
                    PCBKg = Math.Round(pcbKg, 4),
                    OroG = Math.Round(oro, 4),
                    PlataG = Math.Round(plata, 4)
                });
            }

            // LED extracción de oro y plata por unidad PCB
            for (int i = 0; i < cantLED; i++)
            {
                double pcbKg = _distribuciones.GenerarUniforme(0.1, 0.2);
                double oro = (pcbKg * _distribuciones.GenerarNormal(0.15, 0.05))/1000;
                double plata = (pcbKg * _distribuciones.GenerarNormal(0.30, 0.05))/1000;
                oroLote += oro; plataLote += plata; pcbLote += pcbKg;
                extraccionesLED.Add(new ExtraccionUnidad
                {
                    PCBKg = Math.Round(pcbKg, 4),
                    OroG = Math.Round(oro, 4),
                    PlataG = Math.Round(plata, 4)
                });
            }

            _cobreTotalG += cobreLote;
            _oroTotalG += oroLote;
            _plataTotalG += plataLote;
            _pesoTotalPCBKg += pcbLote;

            return new ResultadoExtraccion
            {
                ExtraccionesCRT = extraccionesCRT,
                ExtraccionesLCD = extraccionesLCD,
                ExtraccionesLED = extraccionesLED,
                TotalCobreKG = Math.Round(cobreLote, 2),
                TotalOroKG = Math.Round(oroLote, 4),
                TotalPlataKG = Math.Round(plataLote, 4),
                TotalPCBKg = Math.Round(pcbLote, 4)
            };
        }

        public ResumenExtraccion ObtenerResumen() => new ResumenExtraccion
        {
            CobreTotalKG = Math.Round(_cobreTotalG, 2),
            OroTotalKG = Math.Round(_oroTotalG, 4),
            PlataTotalKG = Math.Round(_plataTotalG, 4),
            PCBTotalKg = Math.Round(_pesoTotalPCBKg, 4)
        };
    }

}
