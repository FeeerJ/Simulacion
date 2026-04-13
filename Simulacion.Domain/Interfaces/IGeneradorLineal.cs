using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Interfaces
{
    public interface IGeneradorLineal
    {
        IEnumerable<double> Generar(long semilla, double a, double c, double m, int cantidad);
    }
}
