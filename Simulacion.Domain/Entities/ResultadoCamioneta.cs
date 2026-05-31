using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResultadoCamioneta
    {
        public int DispositivosTotales { get; set; }      // Total bruto de la camioneta
        public int TVMonitoresFiltrados { get; set; }     // 9.5% del total
        public int Aptos { get; set; }                    // 90% de TVMonitores
        public int Descartados { get; set; }              // 10% de TVMonitores
        public int ParaRefurbishment { get; set; }        // 15% de Aptos
        public int ParaDesmantelamiento { get; set; }     // 85% de Aptos
    }
}
