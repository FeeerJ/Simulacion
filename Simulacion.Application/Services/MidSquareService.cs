using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Simulacion.Domain.Interfaces;


namespace Simulacion.Application.Services
{
    public class MidSquareService : IGenerador
    {
        public IEnumerable<double> Generar(long M, int N, int TOT)
        {
            var resultados = new List<double>();
            long m = M;
            for (int i = 0; i < N; i++)
            {
                long x = m * m;
                string xStr = x.ToString();

                if((xStr.Length - N) %2 != 0)
                {
                    x *= 10;
                    xStr = x.ToString();
                }
                int start = (xStr.Length - N) / 2;
                string centroStr = xStr.Substring(start, N);
                m = long.Parse(centroStr);

                double ri = m/ Math.Pow(10, N);
                resultados.Add(ri);
            }
            return resultados;
        }
    }
}
