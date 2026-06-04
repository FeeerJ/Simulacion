using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResultadoSegmentacion
    {
        public int TotalCRT { get; set; }
        public int TotalLCD { get; set; }
        public int TotalLED { get; set; }
        public int TotalRefurbishment { get; set; }
        public int Total { get; set; }
    }
}

