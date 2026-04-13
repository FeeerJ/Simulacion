using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.Services;

public class CongruentialMethodService
{
    public IEnumerable<double> GenerateAdditive(long M, long N0, long N1, int Total)
    {
        var valores = new List<long>();
        var resultados = new List<double>();

        valores.Add(N0);
        valores.Add(N1);

        for (var i = 2; i < Total + 2; i++)
        {
            long n= (valores[i - 1] + valores[i - 2]) % M;
            valores.Add(n);
        }

        foreach (var v in valores)
        {
            resultados.Add((double)v/M);
        }

        return resultados;

    }

    public IEnumerable<double> GenerateMultiplicative(long M, long N0, long A, int Total)
    {
        var valores = new List<long>();
        var resultados = new List<double>();

        valores.Add(N0);

        for (var i = 1; i < Total + 1; i++)
        {
            long n = (A * valores[i - 1]) % M;
            valores.Add(n);
        }

        foreach (var v in valores)
        {
            resultados.Add((double)v / M);
        }

        return resultados;
    }
}
