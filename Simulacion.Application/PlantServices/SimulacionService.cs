using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
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
        private readonly SustanciasToxicasService _sustanciasToxicas;
        private readonly ExtraccionMaterialesService _extraccion;
        private readonly BalanceFinancieroService _balance;
        private readonly IDistribution _distribution;

        public SimulacionService(LlegadaService llegada, SegmentacionService segmentacion, PesoService peso, SustanciasToxicasService sustanciasToxicas, ExtraccionMaterialesService extraccion, BalanceFinancieroService balance, IDistribution distribution)
        {
            _llegada = llegada;
            _segmentacion = segmentacion;
            _peso = peso;
            _sustanciasToxicas = sustanciasToxicas;
            _extraccion = extraccion;
            _balance = balance;
            _distribution = distribution;
        }

        public ResultadoSimulacion Ejecutar(int dias = 30, int camionetasPorDia = 5)
        {
            var resumenPorDia = new List<ResumenDia>();

            // Estado global de inventario
            int inventarioCRT = 0;
            int inventarioLCD = 0;
            int inventarioLED = 0;
            bool politicaRechazoActiva = false;

            for (int dia = 1; dia <= dias; dia++)
            {
                var resumenDia = new ResumenDia { Dia = dia };

                // Registrar stock inicial
                resumenDia.StockInicialCRT = inventarioCRT;
                resumenDia.StockInicialLCD = inventarioLCD;
                resumenDia.StockInicialLED = inventarioLED;

                for (int camion = 0; camion < camionetasPorDia; camion++)
                {
                    var llegada = _llegada.ProcesarNuevaCamioneta();
                    resumenDia.TotalRefurbishment += llegada.ParaRefurbishment;
                    resumenDia.TotalDescartados += llegada.Descartados;

                    if (!politicaRechazoActiva)
                    {
                        var segmentacion = _segmentacion.ProcesoSegmentar(llegada.ParaDesmantelamiento);
                        inventarioCRT += segmentacion.TotalCRT;
                        inventarioLCD += segmentacion.TotalLCD;
                        inventarioLED += segmentacion.TotalLED;

                        // Comprobar política de rechazo (100% de 375 m2)
                        double espacioOcupado = (inventarioCRT * 0.0525) + (inventarioLCD * 0.08) + (inventarioLED * 0.05);
                        if (espacioOcupado >= 375.0)
                        {
                            politicaRechazoActiva = true;
                        }
                    }
                }

                // Desmantelamiento (Cuello de botella)
                int minutosDisponiblesCRT = 480 * 2; // 2 operarios
                int procesadosCRT = 0;
                while (inventarioCRT > 0)
                {
                    double tiempo = _distribution.GenerarUniforme(25, 45);
                    if (minutosDisponiblesCRT >= tiempo)
                    {
                        minutosDisponiblesCRT -= (int)Math.Ceiling(tiempo);
                        inventarioCRT--;
                        procesadosCRT++;
                    }
                    else
                    {
                        break;
                    }
                }

                int minutosDisponiblesPlanas = 480 * 3; // 3 operarios compartidos
                int procesadosLCD = 0;
                while (inventarioLCD > 0)
                {
                    double tiempo = _distribution.GenerarUniforme(20, 35);
                    if (minutosDisponiblesPlanas >= tiempo)
                    {
                        minutosDisponiblesPlanas -= (int)Math.Ceiling(tiempo);
                        inventarioLCD--;
                        procesadosLCD++;
                    }
                    else
                    {
                        break;
                    }
                }

                int procesadosLED = 0;
                while (inventarioLED > 0)
                {
                    double tiempo = _distribution.GenerarUniforme(15, 25);
                    if (minutosDisponiblesPlanas >= tiempo)
                    {
                        minutosDisponiblesPlanas -= (int)Math.Ceiling(tiempo);
                        inventarioLED--;
                        procesadosLED++;
                    }
                    else
                    {
                        break;
                    }
                }

                // Comprobar si se desactiva la política de rechazo (<= 50% de 375 m2)
                double espacioFinal = (inventarioCRT * 0.0525) + (inventarioLCD * 0.08) + (inventarioLED * 0.05);
                if (politicaRechazoActiva && espacioFinal <= 187.5)
                {
                    politicaRechazoActiva = false;
                }

                // Registrar stock final y ocupación
                resumenDia.StockFinalCRT = inventarioCRT;
                resumenDia.StockFinalLCD = inventarioLCD;
                resumenDia.StockFinalLED = inventarioLED;
                resumenDia.PorcentajeAlmacenamientoOcupado = Math.Round((espacioFinal / 375.0) * 100, 2);

                // Cálculos posteriores basados en lo PROCESADO en el día, no lo llegado
                resumenDia.TotalDesmantelamiento = procesadosCRT + procesadosLCD + procesadosLED;
                resumenDia.TotalCRT = procesadosCRT;
                resumenDia.TotalLCD = procesadosLCD;
                resumenDia.TotalLED = procesadosLED;

                var peso = _peso.CalcularPeso(procesadosCRT, procesadosLCD, procesadosLED);
                var toxicas = _sustanciasToxicas.ProcesarSustancias(peso.PesoTotal);
                var extraccion = _extraccion.ExtraerMateriales(procesadosCRT, procesadosLCD, procesadosLED);

                resumenDia.TotalCobreKG += extraccion.TotalCobreKG;
                resumenDia.TotalOroKG += extraccion.TotalOroKG;
                resumenDia.TotalPlataKG += extraccion.TotalPlataKG;
                resumenDia.TotalPCBKg += extraccion.TotalPCBKg;

                resumenDia.PesoTotalKg += peso.PesoTotal;
                resumenDia.PesoToxiKg += Math.Round(toxicas.PesoToxicoKg, 2);
                resumenDia.CostoDisposicionToxico += toxicas.CostoDisposicion;

                var balance = _balance.ProcesarDia(dia, resumenDia.TotalRefurbishment, resumenDia.PesoTotalKg, resumenDia.TotalCobreKG, resumenDia.TotalPCBKg, resumenDia.CostoDisposicionToxico);
                resumenDia.IngresosDia = balance.IngresosDia;
                resumenDia.CostosDia = balance.CostosDia;
                resumenDia.GananciaDia = balance.GananciaDia;
                resumenDia.PCBacumuladoKg = balance.PCBacumuladoKg;

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
                    TotalPeso = Math.Round(resumenPorDia.Sum(d => d.PesoTotalKg), 2),
                    TotalPesoToxico = Math.Round(resumenPorDia.Sum(d => d.PesoToxiKg), 2),
                    TotalCostoDisposicionToxico = Math.Round(resumenPorDia.Sum(d => d.CostoDisposicionToxico), 2),
                    TotalPesoOro = Math.Round(resumenPorDia.Sum(d => d.TotalOroKG), 2),
                    TotalPesoCobre = Math.Round(resumenPorDia.Sum(d => d.TotalCobreKG), 2),
                    TotalPesoPlata = Math.Round(resumenPorDia.Sum(d => d.TotalPlataKG), 2),
                    IngresosTotales = Math.Round(resumenPorDia.Sum(d => d.IngresosDia), 2),
                    CostosTotales = Math.Round(resumenPorDia.Sum(d => d.CostosDia), 2),
                    GananciaNeta = Math.Round(resumenPorDia.Sum(d => d.GananciaDia), 2)
                }
            };
        }
    }
}
