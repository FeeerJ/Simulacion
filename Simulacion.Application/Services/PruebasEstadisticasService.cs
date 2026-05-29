using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Simulacion.Application.Dtos.PruebasEstadisticasModel;

namespace Simulacion.Application.Services;

public class PruebasEstadisticasService
{
    public PruebaPromediosResultDto PruebaPromedios(List<double> numeros, double zAlfa)
    {
        int n = numeros.Count;
        if (n == 0) throw new ArgumentException("La lista de números no puede estar vacía.");

        // 1. Calcular el promedio aritmético de los números generados
        double promedio = numeros.Average();

        // 2. Determinar el valor del estadístico Z0
        // Numerador: (Promedio - 0.5) * raiz(n)
        double numerador = (promedio - 0.5) * Math.Sqrt(n);

        // Denominador: raiz(1/12)
        double denominador = Math.Sqrt(1.0 / 12.0);

        double z0 = numerador / denominador;

        // 3. Evaluar la condición: |Z0| < Z_alfa
        bool pasaPrueba = Math.Abs(z0) < zAlfa;

        return new PruebaPromediosResultDto
        {
            Promedio = Math.Round(promedio, 5),
            Z0 = Math.Round(z0, 5), // Redondeamos a 5 decimales para mayor legibilidad
            ZAlfa = zAlfa,
            PasaPrueba = pasaPrueba,
            Mensaje = pasaPrueba
                ? "No se rechaza la hipótesis de que los números provienen de un universo uniformemente distribuido."
                : "Se rechaza la hipótesis. Los números NO provienen de un universo uniformemente distribuido."
        };
    }

    // --- Firmas vacías para las demás pruebas (para que el Controller compile) ---

    public PruebaFrecuenciaResultDto PruebaFrecuencia(List<double> numeros, int x, double chiCuadradoAlfa)
    {
        int n = numeros.Count;
        if (n == 0) throw new ArgumentException("La lista de números no puede estar vacía.");
        if (x <= 0) throw new ArgumentException("El número de subintervalos (x) debe ser mayor a cero.");

        // 1. Calcular la Frecuencia Esperada (Fe = n / x)
        double fe = (double)n / x;

        // 2. Determinar las frecuencias observadas (Fo) en cada sub-intervalo
        int[] fo = new int[x];

        foreach (var numero in numeros)
        {
            // Multiplicar por x y truncar nos da el índice del sub-intervalo al que pertenece
            int indice = (int)Math.Floor(numero * x);

            // Caso borde: si el número generado llegara a ser exactamente 1.0, lo colocamos en el último intervalo
            if (indice >= x)
            {
                indice = x - 1;
            }

            fo[indice]++;
        }

        // 3. Calcular la sumatoria para el Chi Cuadrado
        double sumatoria = 0;
        for (int i = 0; i < x; i++)
        {
            sumatoria += Math.Pow(fo[i] - fe, 2);
        }

        // 4. Calcular el Estadístico Chi Cuadrado
        // Fórmula del apunte: Chi^2 = (x / n) * Sumatoria(Fo - Fe)^2
        double chiCuadrado = ((double)x / n) * sumatoria;

        // 5. Evaluar la condición: Chi^2 < Chi^2_alfa
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

        // La fórmula del PDF usa 'n' como el número de PARES generados
        int nPares = cantidadNumeros / 2;

        // 1. Calcular Frecuencia Esperada en cada celda (Fe = nPares / x^2)
        double fe = (double)nPares / (x * x);

        // 2. Determinar las frecuencias observadas (Fo) en cada una de las x^2 celdas
        int[][] fo = new int[x][];
        for (int i = 0; i < x; i++) fo[i] = new int[x];

        for (int i = 0; i < nPares; i++)
        {
            // Tomamos el par de números consecutivos
            double u1 = numeros[i * 2];
            double u2 = numeros[i * 2 + 1];

            // Calculamos a qué celda de la matriz corresponden
            int indiceX = (int)Math.Floor(u1 * x);
            int indiceY = (int)Math.Floor(u2 * x);

            // Caso borde por si el número es exactamente 1.0
            if (indiceX >= x) indiceX = x - 1;
            if (indiceY >= x) indiceY = x - 1;

            fo[indiceX][indiceY]++;
        }

        // 3. Calcular la sumatoria para el Chi Cuadrado
        double sumatoria = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < x; j++)
            {
                sumatoria += Math.Pow(fo[i][j] - fe, 2);
            }
        }

        // 4. Calcular el Estadístico Chi Cuadrado
        // Fórmula del apunte: Chi^2 = (x^2 / n) * Sumatoria(Fo_jk - Fe)^2
        double chiCuadrado = ((double)(x * x) / nPares) * sumatoria;

        // 5. Evaluar la condición: Chi^2 < Chi^2_alfa
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

        // 1 y 2. Generar muestra y ordenar en forma ascendente
        var ordenados = numeros.OrderBy(x => x).ToList();

        List<double> fnx = new List<double>();
        List<double> diferencias = new List<double>();
        double dn = 0;

        // 3 y 4. Calcular Distribución Acumulada y el Estadístico K-S (Max Diferencia)
        for (int i = 1; i <= n; i++)
        {
            // Fn(x) = i / n  (donde i es la posición que ocupa el número, empezando desde 1)
            double acumulada = (double)i / n;
            fnx.Add(Math.Round(acumulada, 5));

            // Diferencia = | Fn(xi) - ui |
            double diferencia = Math.Abs(acumulada - ordenados[i - 1]);
            diferencias.Add(Math.Round(diferencia, 5));

            // Obtener el valor máximo (Dn = Max|Fn(xi) - ui|)
            if (diferencia > dn)
            {
                dn = diferencia;
            }
        }

        dn = Math.Round(dn, 5);

        // 5. Evaluar la condición: Dn < d_alfa,n
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

        // 1 y 2. Generar secuencia binaria (0 si u <= 0.5, 1 si u > 0.5)
        List<int> secuenciaS = numeros.Select(u => u <= 0.5 ? 0 : 1).ToList();
        string secuenciaString = string.Join("", secuenciaS);

        // 3. Determinar las longitudes de corrida
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
                corridaActual = 1; // Reiniciamos el contador para la nueva corrida
            }
        }
        longitudes.Add(corridaActual); // Agregamos la última corrida

        // Determinamos la longitud máxima para saber hasta qué 'i' debemos iterar
        int longitudMaxima = longitudes.Max();

        // Contar las frecuencias observadas (Fo) para cada longitud 'i'
        // Los índices del arreglo van de 0 a (longitudMaxima - 1), representando longitudes de 1 a longitudMaxima
        int[] fo = new int[longitudMaxima];
        foreach (var len in longitudes)
        {
            fo[len - 1]++;
        }

        // 4 y 5. Calcular Fe_i y el Estadístico Chi Cuadrado
        List<double> feList = new List<double>();
        double chiCuadrado = 0;

        for (int i = 1; i <= longitudMaxima; i++)
        {
            // Fórmula: Fe_i = (n - i + 3) / 2^(i + 1)
            double numerador = n - i + 3;
            double denominador = Math.Pow(2, i + 1);
            double fe_i = numerador / denominador;

            feList.Add(Math.Round(fe_i, 5));

            // Fórmula Chi Cuadrado para este elemento: (Fo_i - Fe_i)^2 / Fe_i
            chiCuadrado += Math.Pow(fo[i - 1] - fe_i, 2) / fe_i;
        }

        // 6. Evaluar la condición: Chi^2 < Chi^2_alfa,n
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
