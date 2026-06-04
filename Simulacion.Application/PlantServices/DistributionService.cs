using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class DistributionService: IDistribution
         
    {
        private readonly List<double> _numerosPseudoAleatorios;
        private int _index = 0;

        public DistributionService(IEnumerable<double> numerosGenerados)
        {
            _numerosPseudoAleatorios = numerosGenerados.ToList();
            if (_numerosPseudoAleatorios.Count == 0)
            {
                _numerosPseudoAleatorios.Add(0.5);
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

            double u1 = sacarNumero();
            double u2 = sacarNumero();


            if (u1 == 0) u1 = 0.0001;


            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);


            double resultado = media + (z * desviacionEstandar);


            return Math.Max(0, resultado);
        }
        public double GenerarExponencial(double media)
        {
            double u = sacarNumero();
            if (u == 0) u = 0.0001;


            return -media * Math.Log(1 - u);
        }

        public double GenerarUniforme(double min, double max)
        {
            double u = sacarNumero();
            return min + (max - min) * u;
        }

        public bool EvaluarProbabilidad(double probabilidadExito)
        {

            double u = sacarNumero();
            return u <= probabilidadExito;
        }

        public string DeterminarTecnologia()
        {
            double u = sacarNumero();


            if (u <= 0.15)
                return "CRT";
            else if (u <= 0.65)
                return "LCD";
            else
                return "LED";
        }
    }
}

