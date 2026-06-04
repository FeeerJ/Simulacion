using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResultadoSimulacion
    {
        public int Dias { get; set; }
        public int CamionetasPorDia { get; set; }
        public List<ResumenDia> ResumenPorDia { get; set; }
        public ResumenTotal Totales { get; set; }
  
    }
}

