using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.Dtos
{
    public class LehmerRequestDto
    {
        public long seed { get; set; }
        public long constante { get; set; }
        public int digitos { get; set; }
        public int amount { get; set; }

    }
}
