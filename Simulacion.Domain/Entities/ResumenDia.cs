using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResumenDia
    {
        public int Dia { get; set; }
        public int TotalDesmantelamiento { get; set; }
        public int TotalCRT { get; set; }
        public int TotalLCD { get; set; }
        public int TotalLED { get; set; }
        public int TotalRefurbishment { get; set; }
        public int TotalDescartados { get; set; }
        public double PesoTotalKg { get; set; }
        public double PesoToxiKg { get; set; }
        public double CostoDisposicionToxico { get; set; }

        // Nuevas propiedades para inventario y cuello de botella
        public int StockInicialCRT { get; set; }
        public int StockInicialLCD { get; set; }
        public int StockInicialLED { get; set; }
        public int StockFinalCRT { get; set; }
        public int StockFinalLCD { get; set; }
        public int StockFinalLED { get; set; }
        public double PorcentajeAlmacenamientoOcupado { get; set; }

        public double TotalCobreKG { get; set; }  
        public double TotalOroKG { get; set; }    
        public double TotalPlataKG { get; set; }
        public double TotalPCBKg { get; set; }
        public double IngresosDia { get; set; }
        public double CostosDia { get; set; }     
        public double GananciaDia { get; set; }     
        public double PCBacumuladoKg { get; set; }  
        
        public double UtilizacionOperariosCRT { get; set; }    // % uso operarios CRT en el día
        public double UtilizacionOperariosPlanas { get; set; } // % uso operarios LCD/LED en el día
        public int CamionetasRechazadas { get; set; }          // camionetas rechazadas por almacén lleno
        public bool HuboDespachoFlete { get; set; }
    }
}

