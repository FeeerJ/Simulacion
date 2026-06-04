using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Interfaces
{
    public interface IDistribution
    {
        double GenerarBinomial(double n, double p);
        double GenerarNormal(double media, double desviacionEstandar);
     
        double GenerarExponencial(double media);
        double GenerarUniforme(double min, double max);
        bool EvaluarProbabilidad(double probabilidadExito);
        string DeterminarTecnologia();
    }
}

