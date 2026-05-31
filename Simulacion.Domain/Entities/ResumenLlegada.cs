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
        public int TotalDispositivos { get; set; }
        public double PromedioDispositivosPorCamion { get; set; }
    }
}
