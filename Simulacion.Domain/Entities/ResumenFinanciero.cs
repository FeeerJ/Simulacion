using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResumenFinanciero
    {
        public double IngresosTotales { get; set; }
        public double CostosTotales { get; set; }
        public double GananciaNeta { get; set; }
        public double PCBFinalKg { get; set; }
        public bool SeExportoPCB { get; set; }
    }
}
