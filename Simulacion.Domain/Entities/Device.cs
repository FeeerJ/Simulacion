using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public DeviceType Tipo { get; set; }
        public double HoraLlegada { get; set; }
       // public bool isApto { get; set; }
        public double Peso { get; set; }
    }
}
