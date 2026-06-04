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

        // Ejecuta la simulación de la planta por un número determinado de días
        // Retorna un objeto ResultadoSimulacion con el resumen de cada día y los totales
        public ResultadoSimulacion Ejecutar(
            int dias = 30,
            int camionetasPorDia = 5,
            double capacidadAlmacenM3 = 375.0,
            int operariosCRT = 2,
            int operariosPlanas = 3)
        {
            var resumenPorDia = new List<ResumenDia>();

            // Inventario inicial en 0
            int inventarioCRT = 0;
            int inventarioLCD = 0;
            int inventarioLED = 0;
            // Política para detener ingreso de camionetas si el almacén está lleno
            bool politicaRechazoActiva = false;

            // Bucle principal: simula la operación de la planta día a día
            for (int dia = 1; dia <= dias; dia++)
            {
                var resumenDia = new ResumenDia { Dia = dia };





                // Calcular volumen actual del inventario
                double areaInicial = (inventarioCRT * 0.21)
                                   + (inventarioLCD * 0.08)
                                   + (inventarioLED * 0.05);

                // Si se había alcanzado el límite, revisar si el inventario bajó al 50% para volver a recibir
                if (politicaRechazoActiva && areaInicial <= capacidadAlmacenM3 * 0.5)
                    politicaRechazoActiva = false;


                resumenDia.StockInicialCRT = inventarioCRT;
                resumenDia.StockInicialLCD = inventarioLCD;
                resumenDia.StockInicialLED = inventarioLED;




                // Procesamiento de llegada de cada camioneta programada en el día
                for (int camion = 0; camion < camionetasPorDia; camion++)
                {
                    // Revisar capacidad nuevamente por si se llenó con camionetas previas de este mismo día
                    double areaActual = (inventarioCRT * 0.21)
                                      + (inventarioLCD * 0.08)
                                      + (inventarioLED * 0.05);

                    if (politicaRechazoActiva || areaActual >= capacidadAlmacenM3)
                    {
                        // Si el almacén está lleno, se rechaza la camioneta
                        politicaRechazoActiva = true;
                        resumenDia.CamionetasRechazadas++;
                        continue;
                    }

                    // Registrar llegada y equipos descartados
                    var llegada = _llegada.ProcesarNuevaCamioneta();
                    resumenDia.TotalDescartados += llegada.Descartados;

                    // Segmentar equipos aceptados para desmantelamiento vs refurbishment
                    var segmentacion = _segmentacion.ProcesoSegmentar(llegada.ParaDesmantelamiento);
                    resumenDia.TotalRefurbishment += segmentacion.TotalRefurbishment;
                    
                    // Añadir al inventario los equipos listos para desmantelar
                    inventarioCRT += segmentacion.TotalCRT;
                    inventarioLCD += segmentacion.TotalLCD;
                    inventarioLED += segmentacion.TotalLED;
                }



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



                double areaFinal = (inventarioCRT * 0.21)
                                 + (inventarioLCD * 0.08)
                                 + (inventarioLED * 0.05);



                int minutosTotalesCRT = 480 * operariosCRT;
                int minutosTotalesPlanas = 480 * operariosPlanas;

                resumenDia.UtilizacionOperariosCRT = Math.Round(
                    (double)(minutosTotalesCRT - minutosDisponiblesCRT) / minutosTotalesCRT * 100, 2);

                resumenDia.UtilizacionOperariosPlanas = Math.Round(
                    (double)(minutosTotalesPlanas - minutosDisponiblesPlanas) / minutosTotalesPlanas * 100, 2);


                resumenDia.StockFinalCRT = inventarioCRT;
                resumenDia.StockFinalLCD = inventarioLCD;
                resumenDia.StockFinalLED = inventarioLED;
                resumenDia.PorcentajeAlmacenamientoOcupado = Math.Round((areaFinal / capacidadAlmacenM3) * 100, 2);


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
                resumenDia.HuboDespachoFlete = balance.HuboDespachoFlete;

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

