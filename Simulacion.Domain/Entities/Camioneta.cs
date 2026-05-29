using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class Camioneta
    {
        /*CADA DEVICE ES UN LOTE DE RESIDUOS - 1 CAMIONETA */

        //public DeviceType Tipo { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public double? PesoAprovechable { get; set; }
        public double? PesoDesmantelamiento { get; set; }
        public double? PesoDescarte { get; set; }
        public double PesoTotal { get; set; }
        public double? PesoReventa { get; set; }

        public string Estado { get; set; }
    }
}
