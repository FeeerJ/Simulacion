using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class Operator
    {
        public int Id { get; set; }
        public bool isActivo { get; set; }
        public double CoeficienteEficiencia { get; set; }
        public double CostoHora { get; set; }
    }
}

