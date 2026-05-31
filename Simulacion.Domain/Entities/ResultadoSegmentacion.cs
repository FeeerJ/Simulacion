using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResultadoSegmentacion
    {
        public int TotalCRT { get; set; }   // 15%
        public int TotalLCD { get; set; }   // 50%
        public int TotalLED { get; set; }   // 35%
        public int Total { get; set; }
    }
}
