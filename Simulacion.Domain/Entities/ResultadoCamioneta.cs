using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResultadoCamioneta
    {
        public int DispositivosTotales { get; set; }
        public int TVMonitoresFiltrados { get; set; }
        public int Aptos { get; set; }
        public int Descartados { get; set; }
        public int ParaRefurbishment { get; set; }
        public int ParaDesmantelamiento { get; set; }
    }
}

