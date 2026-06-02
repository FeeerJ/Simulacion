using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class DistributionService: IDistribution
         /*Se define como se van a calcular las diversas distribuciones de probabalidad, en base al metodo de generacion pseudoaleatorio definido en el DiagnosticsController*/
    {
        private readonly List<double> _numerosPseudoAleatorios;
        private int _index = 0;

        public DistributionService(IEnumerable<double> numerosGenerados)
        {
            _numerosPseudoAleatorios = numerosGenerados.ToList();
            if (_numerosPseudoAleatorios.Count == 0)
            {
                _numerosPseudoAleatorios.Add(0.5); // Prevenir división por cero o lista vacía
            }
        }

        private double sacarNumero()
        {
            double u = _numerosPseudoAleatorios[_index];
            _index = (_index + 1) % _numerosPseudoAleatorios.Count;
            return u;
        }

        public double GenerarBinomial(double n, double p)
        {
            /*n = total de elementos - p = probabilidad de exito*/
            int exitos = 0;
            for (int i = 0; i < n; i++)
            {
                double u = sacarNumero();
                if (u < p)
                {
                    exitos++;
                }
            }
            return exitos;
        }
        public double GenerarNormal(double media, double desviacionEstandar)
        {
            // Box-Muller requiere dos variables U(0,1)
            double u1 = sacarNumero();
            double u2 = sacarNumero();

            // Evitamos que u1 sea exactamente 0 para que el Logaritmo no explote
            if (u1 == 0) u1 = 0.0001;

            // Fórmula de Box-Muller
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

            // Transformamos Z a nuestra media y desviación estándar
            double resultado = media + (z * desviacionEstandar);

            // Evitamos pesos negativos por seguridad
            return Math.Max(0, resultado);
        }
        public double GenerarExponencial(double media)
        {
            double u = sacarNumero();
            if (u == 0) u = 0.0001;

            // Método de la Transformada Inversa para la Exponencial
            return -media * Math.Log(1 - u);
        }

        public double GenerarUniforme(double min, double max)
        {
            double u = sacarNumero();
            return min + (max - min) * u;
        }

        public bool EvaluarProbabilidad(double probabilidadExito)
        {
            // Ejemplo: Para el 90% Apto, pasamos 0.90 como parámetro
            double u = sacarNumero();
            return u <= probabilidadExito;
        }

        public string DeterminarTecnologia()
        {
            double u = sacarNumero();

            // Segmentación: 15% CRT / 50% LCD / 35% LED
            if (u <= 0.15)
                return "CRT";
            else if (u <= 0.65) // 0.15 + 0.50
                return "LCD";
            else
                return "LED";
        }
    }
}
