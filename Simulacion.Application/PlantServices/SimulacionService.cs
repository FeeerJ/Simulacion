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

        public ResultadoSimulacion Ejecutar(
            int dias = 30,
            int camionetasPorDia = 5,
            double capacidadAlmacenM3 = 375.0,
            int operariosCRT = 2,
            int operariosPlanas = 3)
        {
            var resumenPorDia = new List<ResumenDia>();

            int inventarioCRT = 0;
            int inventarioLCD = 0;
            int inventarioLED = 0;
            bool politicaRechazoActiva = false;

            for (int dia = 1; dia <= dias; dia++)
            {
                var resumenDia = new ResumenDia { Dia = dia };

                // Stock al inicio del día (remanente del día anterior)
                resumenDia.StockInicialCRT = inventarioCRT;
                resumenDia.StockInicialLCD = inventarioLCD;
                resumenDia.StockInicialLED = inventarioLED;

                // ── RECEPCIÓN DE CAMIONETAS ──────────────────────────────────────
                // Se evalúa el espacio DESPUÉS de cada camioneta para reflejar
                // la mecánica de descuento de espacio en tiempo real del modelo verbal
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

                        // Área en piso por unidad (m²) con apilamiento aplicado
                        // CRT: volumen 0.21m³ / altura torre 1.80m (4 niveles × 0.45m) = 0.1167 m²
                        // LCD: volumen 0.08m³ / altura torre 0.45m (3 niveles × 0.15m) = 0.1778 m²
                        // LED: volumen 0.05m³ / altura torre 0.36m (3 niveles × 0.12m) = 0.1389 m²
                        double areaOcupada = (inventarioCRT * 0.1167)
                                           + (inventarioLCD * 0.1778)
                                           + (inventarioLED * 0.1389);

                        if (areaOcupada >= capacidadAlmacenM3)
                        {
                            politicaRechazoActiva = true;
                            resumenDia.CamionetasRechazadas++;
                        }
                    }
                    else
                    {
                        resumenDia.CamionetasRechazadas++;
                    }
                }

                // ── DESMANTELAMIENTO CRT ─────────────────────────────────────────
                // 2 operarios, tiempo UNIF(20, 40) min según modelo verbal
                int minutosDisponiblesCRT = 480 * operariosCRT;
                int procesadosCRT = 0;
                while (inventarioCRT > 0)
                {
                    double tiempo = _distribution.GenerarUniforme(20, 40);
                    if (minutosDisponiblesCRT >= tiempo)
                    {
                        minutosDisponiblesCRT -= (int)Math.Ceiling(tiempo);
                        inventarioCRT--;
                        procesadosCRT++;
                    }
                    else break;
                }

                // ── DESMANTELAMIENTO LCD/LED ─────────────────────────────────────
                // 3 operarios compartidos, tiempo UNIF(10, 15) min según modelo verbal
                int minutosDisponiblesPlanas = 480 * operariosPlanas;
                int procesadosLCD = 0;
                while (inventarioLCD > 0)
                {
                    double tiempo = _distribution.GenerarUniforme(10, 15);
                    if (minutosDisponiblesPlanas >= tiempo)
                    {
                        minutosDisponiblesPlanas -= (int)Math.Ceiling(tiempo);
                        inventarioLCD--;
                        procesadosLCD++;
                    }
                    else break;
                }

                int procesadosLED = 0;
                while (inventarioLED > 0)
                {
                    double tiempo = _distribution.GenerarUniforme(10, 15);
                    if (minutosDisponiblesPlanas >= tiempo)
                    {
                        minutosDisponiblesPlanas -= (int)Math.Ceiling(tiempo);
                        inventarioLED--;
                        procesadosLED++;
                    }
                    else break;
                }

                // ── POLÍTICA DE REAPERTURA ───────────────────────────────────────
                // Se rehabilita la recepción cuando el stock baja al 50% de capacidad
                double areaFinal = (inventarioCRT * 0.1167)
                                 + (inventarioLCD * 0.1778)
                                 + (inventarioLED * 0.1389);

                if (politicaRechazoActiva && areaFinal <= capacidadAlmacenM3 * 0.5)
                    politicaRechazoActiva = false;

                // ── MÉTRICAS DE CUELLO DE BOTELLA ────────────────────────────────
                // Utilización = minutos consumidos / minutos totales disponibles × 100
                int minutosTotalesCRT = 480 * operariosCRT;
                int minutosTotalesPlanas = 480 * operariosPlanas;

                resumenDia.UtilizacionOperariosCRT = Math.Round(
                    (double)(minutosTotalesCRT - minutosDisponiblesCRT) / minutosTotalesCRT * 100, 2);

                resumenDia.UtilizacionOperariosPlanas = Math.Round(
                    (double)(minutosTotalesPlanas - minutosDisponiblesPlanas) / minutosTotalesPlanas * 100, 2);

                // ── STOCK FINAL Y OCUPACIÓN ──────────────────────────────────────
                resumenDia.StockFinalCRT = inventarioCRT;
                resumenDia.StockFinalLCD = inventarioLCD;
                resumenDia.StockFinalLED = inventarioLED;
                resumenDia.PorcentajeAlmacenamientoOcupado = Math.Round((areaFinal / capacidadAlmacenM3) * 100, 2);

                // ── CÁLCULOS SOBRE LO PROCESADO ──────────────────────────────────
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

                var balance = _balance.ProcesarDia(
                    dia,
                    resumenDia.TotalRefurbishment,
                    resumenDia.PesoTotalKg,
                    resumenDia.TotalCobreKG,
                    resumenDia.TotalPCBKg,
                    resumenDia.CostoDisposicionToxico);

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
                    GananciaNeta = Math.Round(resumenPorDia.Sum(d => d.GananciaDia), 2),
                    // ── NUEVOS: métricas de cuello de botella ──
                    UtilizacionPromedioCRT = Math.Round(
                        resumenPorDia.Average(d => d.UtilizacionOperariosCRT), 2),
                    UtilizacionPromedioPlanas = Math.Round(
                        resumenPorDia.Average(d => d.UtilizacionOperariosPlanas), 2),
                    DiasSaturacionCRT = resumenPorDia.Count(d => d.StockFinalCRT > 0),
                    DiasSaturacionPlanas = resumenPorDia.Count(d => d.StockFinalLCD > 0
                                                                 || d.StockFinalLED > 0),
                    TotalCamionetasRechazadas = resumenPorDia.Sum(d => d.CamionetasRechazadas)
                }
            };
        }
    }
}
