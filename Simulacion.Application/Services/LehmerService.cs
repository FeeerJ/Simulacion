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
                    // Tomamos las primeras k posiciones
                    string parteAltaStr = xStr.Substring(0, k);
                    // Tomamos lo que queda a partir de k
                    string parteBajaStr = xStr.Substring(k);

                    long parteAlta = long.Parse(parteAltaStr);
                    long parteBaja = long.Parse(parteBajaStr);

                    // Nueva semilla es la resta según tu regla
                    m = parteBaja - parteAlta;
                }
                else
                {
                    // Si el número es muy chico, el algoritmo podría converger o fallar
                    m = x;
                }

                // Para el número pseudoaleatorio, normalizamos (u_i)
                // Usualmente se divide por el valor máximo posible o se trata según el libro
                resultados.Add(m);
                /**/
            }
            return resultados;
        }
    }
}
