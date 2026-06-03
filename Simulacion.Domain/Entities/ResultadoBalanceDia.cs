using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResultadoBalanceDia
    {
        public int Dia { get; set; }
        public double IngresosDia { get; set; }
        public double CostosDia { get; set; }
        public double GananciaDia { get; set; }
        public double PCBacumuladoKg { get; set; }
        public double IngresosTotalesAcumulados { get; set; }
        public double CostosTotalesAcumulados { get; set; }
        public double GananciaNetaAcumulada { get; set; }
        public bool HuboDespachoFlete { get; set; }
    }
}
