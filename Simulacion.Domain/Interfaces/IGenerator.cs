using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Interfaces
{
    public interface IGenerator
    {
        IEnumerable<Double> Generar(long seed, int digitos, int cantidad);
    }
}

