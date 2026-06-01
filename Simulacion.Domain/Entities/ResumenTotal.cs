using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResumenTotal
    {
        public int TotalCRT { get; set; }
        public int TotalLCD { get; set; }
        public int TotalLED { get; set; }
        public int TotalRefurbishment { get; set; }
        public int TotalDescartados { get; set; }
        public int TotalDesmantelamiento { get; set; }
        public double TotalPeso { get; set; }
       public double TotalPesoToxico { get; set; }
          public double TotalCostoDisposicionToxico { get; set; }
        public double TotalPesoOro { get; set; }
        public double TotalPesoPlata { get; set; }
        public double TotalPesoCobre { get; set; }
        public double IngresosTotales { get; set; }
        public double CostosTotales { get; set; }    
        public double GananciaNeta { get; set; }     
    }
}
