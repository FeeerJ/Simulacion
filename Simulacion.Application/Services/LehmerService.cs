using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Simulacion.Application.Services
{
    public class LehmerService : IGenerator
    {
        public IEnumerable<Double> Generar(long semilla, int digitos, int cantidad)
        {
            return (GenerarLehmerDesplazamiento(semilla, 19, digitos, cantidad));
        }

        public IEnumerable<double> GenerarLehmerDesplazamiento(long semilla, long t, int k, int cantidad)
        {
            var resultados = new List<Double>();
            long m = semilla;

            for(int i = 0; i < cantidad; i++)
            {
                long x = m * t;
                string xStr = x.ToString();

                if (xStr.Length > k)
                {

                    string parteAltaStr = xStr.Substring(0, k);

                    string parteBajaStr = xStr.Substring(k);

                    long parteAlta = long.Parse(parteAltaStr);
                    long parteBaja = long.Parse(parteBajaStr);


                    m = parteBaja - parteAlta;
                }
                else
                {

                    m = x;
                }

                double ri = (double)m / Math.Pow(10, k);
                resultados.Add(ri);
                
            }
            return resultados;
        }
    }
}

