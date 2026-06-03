using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResumenLlegada
    {
        public int TotalCamiones { get; set; }
        public int TotalDispositivosBrutos { get; set; }
        public int TotalTVMonitores { get; set; }
        public int TotalAptos { get; set; }
        public int TotalDescartados { get; set; }
        public int TotalRefurbishment { get; set; }
        public int TotalDesmantelamiento { get; set; }
    }
}
