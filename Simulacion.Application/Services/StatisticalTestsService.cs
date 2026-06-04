using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Simulacion.Application.Dtos.PruebasEstadisticasRequesDto;

namespace Simulacion.Application.Services;

public class StatisticalTestsService
{
    public PruebaPromediosResultDto PruebaPromedios(List<double> numeros, double zAlfa)
    {
        int n = numeros.Count;
        if (n == 0) throw new ArgumentException("La lista de números no puede estar vacía.");


        double promedio = numeros.Average();



        double numerador = (promedio - 0.5) * Math.Sqrt(n);


        double denominador = Math.Sqrt(1.0 / 12.0);

        double z0 = numerador / denominador;


        bool pasaPrueba = Math.Abs(z0) < zAlfa;

        return new PruebaPromediosResultDto
        {
            Promedio = Math.Round(promedio, 5),
            Z0 = Math.Round(z0, 5),
            ZAlfa = zAlfa,
            PasaPrueba = pasaPrueba,
            Mensaje = pasaPrueba
                ? "No se rechaza la hipótesis de que los números provienen de un universo uniformemente distribuido."
                : "Se rechaza la hipótesis. Los números NO provienen de un universo uniformemente distribuido."
        };
    }



    public PruebaFrecuenciaResultDto PruebaFrecuencia(List<double> numeros, int x, double chiCuadradoAlfa)
    {
        int n = numeros.Count;
        if (n == 0) throw new ArgumentException("La lista de números no puede estar vacía.");
        if (x <= 0) throw new ArgumentException("El número de subintervalos (x) debe ser mayor a cero.");


        double fe = (double)n / x;


        int[] fo = new int[x];

        foreach (var numero in numeros)
        {

            int indice = (int)Math.Floor(numero * x);


            if (indice >= x)
            {
                indice = x - 1;
            }

            fo[indice]++;
        }


        double sumatoria = 0;
        for (int i = 0; i < x; i++)
        {
            sumatoria += Math.Pow(fo[i] - fe, 2);
        }



        double chiCuadrado = ((double)x / n) * sumatoria;


        bool pasaPrueba = chiCuadrado < chiCuadradoAlfa;

        return new PruebaFrecuenciaResultDto
        {
            FrecuenciasObservadas = fo.ToList(),
            FrecuenciaEsperada = fe,
            ChiCuadrado = Math.Round(chiCuadrado, 5),
            ChiCuadradoAlfa = chiCuadradoAlfa,
            PasaPrueba = pasaPrueba,
            Mensaje = pasaPrueba
                ? "No se rechaza la hipótesis de que los números provienen de un universo uniformemente distribuido."
                : "Se rechaza la hipótesis. Los números NO provienen de un universo uniformemente distribuido."
        };
    }

    public PruebaSerieResultDto PruebaSerie(List<double> numeros, int x, double chiCuadradoAlfa)
    {
        int cantidadNumeros = numeros.Count;
        if (cantidadNumeros < 2) throw new ArgumentException("Se necesitan al menos 2 números para formar un par.");
        if (x <= 0) throw new ArgumentException("El valor de subdivisiones (x) debe ser mayor a cero.");


        int nPares = cantidadNumeros / 2;


        double fe = (double)nPares / (x * x);


        int[][] fo = new int[x][];
        for (int i = 0; i < x; i++) fo[i] = new int[x];

        for (int i = 0; i < nPares; i++)
        {

            double u1 = numeros[i * 2];
            double u2 = numeros[i * 2 + 1];


            int indiceX = (int)Math.Floor(u1 * x);
            int indiceY = (int)Math.Floor(u2 * x);


            if (indiceX >= x) indiceX = x - 1;
            if (indiceY >= x) indiceY = x - 1;

            fo[indiceX][indiceY]++;
        }


        double sumatoria = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < x; j++)
            {
                sumatoria += Math.Pow(fo[i][j] - fe, 2);
            }
        }



        double chiCuadrado = ((double)(x * x) / nPares) * sumatoria;


        bool pasaPrueba = chiCuadrado < chiCuadradoAlfa;

        return new PruebaSerieResultDto
        {
            FrecuenciasObservadas = fo,
            FrecuenciaEsperada = Math.Round(fe, 5),
            ChiCuadrado = Math.Round(chiCuadrado, 5),
            ChiCuadradoAlfa = chiCuadradoAlfa,
            PasaPrueba = pasaPrueba,
            Mensaje = pasaPrueba
                ? "No se rechaza la hipótesis de que los números provienen de un universo uniformemente distribuido."
                : "Se rechaza la hipótesis. Los números NO provienen de un universo uniformemente distribuido."
        };
    }

    public PruebaKSResultDto PruebaKS(List<double> numeros, double dAlfaN)
    {
        int n = numeros.Count;
        if (n == 0) throw new ArgumentException("La lista de números no puede estar vacía.");


        var ordenados = numeros.OrderBy(x => x).ToList();

        List<double> fnx = new List<double>();
        List<double> diferencias = new List<double>();
        double dn = 0;


        for (int i = 1; i <= n; i++)
        {

            double acumulada = (double)i / n;
            fnx.Add(Math.Round(acumulada, 5));


            double diferencia = Math.Abs(acumulada - ordenados[i - 1]);
            diferencias.Add(Math.Round(diferencia, 5));


            if (diferencia > dn)
            {
                dn = diferencia;
            }
        }

        dn = Math.Round(dn, 5);


        bool pasaPrueba = dn < dAlfaN;

        return new PruebaKSResultDto
        {
            NumerosOrdenados = ordenados,
            DistribucionAcumulada = fnx,
            Diferencias = diferencias,
            Dn = dn,
            DAlfaN = dAlfaN,
            PasaPrueba = pasaPrueba,
            Mensaje = pasaPrueba
                ? "No se rechaza la hipótesis de que los números provienen de un universo uniformemente distribuido."
                : "Se rechaza la hipótesis. Los números NO provienen de un universo uniformemente distribuido."
        };
    }

    public PruebaCorridasResultDto PruebaCorridasMedia(List<double> numeros, double chiCuadradoAlfa)
    {
        int n = numeros.Count;
        if (n == 0) throw new ArgumentException("La lista de números no puede estar vacía.");


        List<int> secuenciaS = numeros.Select(u => u <= 0.5 ? 0 : 1).ToList();
        string secuenciaString = string.Join("", secuenciaS);


        List<int> longitudes = new List<int>();
        int corridaActual = 1;

        for (int i = 1; i < n; i++)
        {
            if (secuenciaS[i] == secuenciaS[i - 1])
            {
                corridaActual++;
            }
            else
            {
                longitudes.Add(corridaActual);
                corridaActual = 1;
            }
        }
        longitudes.Add(corridaActual);


        int longitudMaxima = longitudes.Max();



        int[] fo = new int[longitudMaxima];
        foreach (var len in longitudes)
        {
            fo[len - 1]++;
        }


        List<double> feList = new List<double>();
        double chiCuadrado = 0;

        for (int i = 1; i <= longitudMaxima; i++)
        {

            double numerador = n - i + 3;
            double denominador = Math.Pow(2, i + 1);
            double fe_i = numerador / denominador;

            feList.Add(Math.Round(fe_i, 5));


            chiCuadrado += Math.Pow(fo[i - 1] - fe_i, 2) / fe_i;
        }


        bool pasaPrueba = chiCuadrado < chiCuadradoAlfa;

        return new PruebaCorridasResultDto
        {
            SecuenciaBinaria = secuenciaString,
            LongitudesCorridas = longitudes,
            FrecuenciasObservadas = fo.ToList(),
            FrecuenciasEsperadas = feList,
            ChiCuadrado = Math.Round(chiCuadrado, 5),
            ChiCuadradoAlfa = chiCuadradoAlfa,
            PasaPrueba = pasaPrueba,
            Mensaje = pasaPrueba
                ? "No se rechaza la hipótesis de que los números provienen de un universo uniformemente distribuido."
                : "Se rechaza la hipótesis. Los números NO provienen de un universo uniformemente distribuido."
        };
    }
}

