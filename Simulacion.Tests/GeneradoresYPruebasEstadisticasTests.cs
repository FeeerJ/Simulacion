using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Simulacion.Application.Services;
using static Simulacion.Application.Dtos.PruebasEstadisticasRequesDto;

namespace Simulacion.Tests;

/// <summary>
/// Clase de pruebas unitarias para validar tanto la correcta implementación matemática
/// de las pruebas estadísticas como la calidad de los generadores de números pseudoaleatorios.
/// </summary>
public class GeneradoresYPruebasEstadisticasTests
{
    private readonly CongruentialMethodService _generador;
    private readonly StatisticalTestsService _pruebasService;
    private readonly ITestOutputHelper _output;

    /// <summary>
    /// Constructor de la clase de pruebas.
    /// Registra los servicios bajo prueba y configura ITestOutputHelper para poder
    /// imprimir los valores calculados en la consola del Explorador de Pruebas de Visual Studio.
    /// </summary>
    public GeneradoresYPruebasEstadisticasTests(ITestOutputHelper output)
    {
        _generador = new CongruentialMethodService();
        _pruebasService = new StatisticalTestsService();
        _output = output;
    }

    /// <summary>
    /// PRUEBA 1: Validación básica de límites matemáticos del generador mixto.
    /// OBJETIVO: Asegurar que el generador devuelva la cantidad exacta de números solicitados
    /// y que todos se encuentren dentro del rango matemático requerido [0.0, 1.0).
    /// CÓMO SE PRUEBA: Se ejecuta el generador lineal mixto con parámetros sencillos y 
    /// se inspecciona que ningún número generado sea menor a 0 ni mayor o igual a 1.
    /// </summary>
    [Fact]
    public void GeneradorMixto_DebeGenerarValoresEnRangoCeroAUno()
    {
        // Arrange (Preparación) - Parámetros del generador
        long m = 100000;
        long semilla = 1021;
        long a = 3;
        long c = 99000;
        int total = 1000;

        // Act (Ejecución) - Generación de la lista de números
        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).ToList();

        // Assert (Verificación)
        // Se espera total + 1 elementos porque el método incluye el estado inicial (N0) como primer elemento
        Assert.Equal(total + 1, numeros.Count);
        // Validamos que cada elemento individual ri cumpla con la restricción matemática: 0 <= ri < 1
        Assert.All(numeros, n => Assert.True(n >= 0.0 && n < 1.0, $"El número {n} está fuera de los límites [0, 1)"));
    }

    /// <summary>
    /// PRUEBA 2: Validación matemática de la lógica de "Prueba de Promedios" con un conjunto ideal.
    /// OBJETIVO: Probar que la fórmula de la prueba de promedios esté bien implementada.
    /// CÓMO SE PRUEBA: Se ingresa una lista balanceada y simétrica cuyo promedio es exactamente 0.5.
    /// El estadístico Z0 resultante debe ser exactamente 0.0, lo cual cae dentro de cualquier zona
    /// de no rechazo (por ejemplo, con Z_alfa = 1.96). La prueba debe ser aprobada (PasaPrueba = true).
    /// </summary>
    [Fact]
    public void PruebaPromedios_ConDistribuciónUniformePerfecta_DebePasarLaPrueba()
    {
        // Arrange (Preparación) - Datos ideales de entrada
        var numerosDistribuidos = new List<double> { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };
        double zAlfa = 1.96; // Valor crítico Z para 95% de confianza (alfa = 0.05)

        // Act (Ejecución) - Ejecutar cálculo estadístico
        PruebaPromediosResultDto resultado = _pruebasService.PruebaPromedios(numerosDistribuidos, zAlfa);

        // Assert (Verificación)
        Assert.True(resultado.PasaPrueba, $"La prueba de promedios falló inesperadamente: {resultado.Mensaje}");
        Assert.Equal(0.5, resultado.Promedio);
        Assert.Equal(0.0, resultado.Z0);
    }

    /// <summary>
    /// PRUEBA 3: Validación matemática de detección de sesgos en la "Prueba de Promedios".
    /// OBJETIVO: Verificar que la prueba estadística sea capaz de discriminar y rechazar
    /// secuencias que no son uniformes.
    /// CÓMO SE PRUEBA: Se inyecta una lista con valores muy cercanos a 0.0. El promedio se desviará
    /// fuertemente de 0.5 y el estadístico Z0 excederá el límite crítico. Se verifica que PasaPrueba = false.
    /// </summary>
    [Fact]
    public void PruebaPromedios_ConValoresSesgados_DebeRechazarLaHipotesis()
    {
        // Arrange (Preparación) - Datos no uniformes
        var numerosSesgados = new List<double> { 0.01, 0.02, 0.05, 0.03, 0.01, 0.04, 0.02 };
        double zAlfa = 1.96;

        // Act (Ejecución)
        PruebaPromediosResultDto resultado = _pruebasService.PruebaPromedios(numerosSesgados, zAlfa);

        // Assert (Verificación)
        Assert.False(resultado.PasaPrueba, "Se esperaba que la prueba fallara debido al fuerte sesgo.");
    }

    // =========================================================================
    // PRUEBAS DE CALIDAD: ¿EL GENERADOR SUPERA LAS PRUEBAS ESTADÍSTICAS?
    // =========================================================================

    /// <summary>
    /// PRUEBA 4: Prueba de Promedios sobre el Generador Mixto.
    /// OBJETIVO: Evaluar si el promedio de los números generados es estadísticamente igual a 0.5.
    /// CÓMO SE PRUEBA: Se generan 5000 números con los parámetros de glibc. Se descarta la semilla inicial
    /// para evaluar únicamente el comportamiento de los números pseudoaleatorios generados.
    /// Se verifica que el Z0 absoluto calculado sea menor que 1.96.
    /// </summary>
    [Fact]
    public void GeneradorMixto_DebePasar_PruebaPromedios()
    {
        // Arrange (Preparación) - Parámetros robustos de glibc
        long m = 2147483648; // 2^31
        long semilla = 123456789;
        long a = 1103515245;
        long c = 12345;
        int total = 5000;
        double zAlfa = 1.96; // Valor crítico de Z para alfa = 0.05

        // Act (Ejecución) - Generar y evaluar
        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).Skip(1).ToList(); 
        var resultado = _pruebasService.PruebaPromedios(numeros, zAlfa);

        // Imprimimos los resultados en el log del Explorador de Pruebas
        _output.WriteLine($"[Promedios] Promedio: {resultado.Promedio}, Z0: {resultado.Z0}, Pasa: {resultado.PasaPrueba}");

        // Assert (Verificación)
        Assert.True(resultado.PasaPrueba, $"El generador falló la prueba de promedios: {resultado.Mensaje}");
    }

    /// <summary>
    /// PRUEBA 5: Prueba de Frecuencia sobre el Generador Mixto.
    /// OBJETIVO: Evaluar si los números generados se distribuyen uniformemente a lo largo de subintervalos.
    /// CÓMO SE PRUEBA: Se generan 5000 números y se dividen en 10 subintervalos de tamaño 0.1.
    /// Se calcula el estadístico Chi-Cuadrado sumando las desviaciones cuadráticas.
    /// Se verifica que el estadístico obtenido sea menor que el valor de tabla crítico 16.919.
    /// </summary>
    [Fact]
    public void GeneradorMixto_DebePasar_PruebaFrecuencia()
    {
        // Arrange (Preparación)
        long m = 2147483648;
        long semilla = 123456789;
        long a = 1103515245;
        long c = 12345;
        int total = 5000;
        
        int subintervalos = 10;
        double chiCuadradoAlfa = 16.919; // Chi-Cuadrado de tabla para v = 9 grados de libertad (10 - 1) y alfa = 0.05

        // Act (Ejecución)
        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).Skip(1).ToList();
        var resultado = _pruebasService.PruebaFrecuencia(numeros, subintervalos, chiCuadradoAlfa);

        _output.WriteLine($"[Frecuencia] Chi2: {resultado.ChiCuadrado}, Chi2-Critico: {resultado.ChiCuadradoAlfa}, Pasa: {resultado.PasaPrueba}");

        // Assert (Verificación)
        Assert.True(resultado.PasaPrueba, $"El generador falló la prueba de frecuencia: {resultado.Mensaje}");
    }

    /// <summary>
    /// PRUEBA 6: Prueba de Series sobre el Generador Mixto.
    /// OBJETIVO: Evaluar la independencia en 2D entre números consecutivos (pares correlacionados).
    /// CÓMO SE PRUEBA: Se toman los 5000 números en pares consecutivos (X, Y) y se ubican en una cuadrícula
    /// de 5x5 (25 celdas). Se verifica mediante Chi-Cuadrado que los puntos estén distribuidos de forma homogénea.
    /// El estadístico calculado debe ser inferior al valor de tabla crítico para 24 grados de libertad (25 - 1): 36.415.
    /// </summary>
    [Fact]
    public void GeneradorMixto_DebePasar_PruebaSerie()
    {
        // Arrange (Preparación)
        long m = 2147483648;
        long semilla = 123456789;
        long a = 1103515245;
        long c = 12345;
        int total = 5000;
        
        int divisionesMatriz = 5; // Genera una cuadrícula de 5x5 = 25 celdas
        double chiCuadradoAlfa = 36.415; // Chi-Cuadrado de tabla para v = 24 grados de libertad (25 - 1) y alfa = 0.05

        // Act (Ejecución)
        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).Skip(1).ToList();
        var resultado = _pruebasService.PruebaSerie(numeros, divisionesMatriz, chiCuadradoAlfa);

        _output.WriteLine($"[Serie] Chi2: {resultado.ChiCuadrado}, Chi2-Critico: {resultado.ChiCuadradoAlfa}, Pasa: {resultado.PasaPrueba}");

        // Assert (Verificación)
        Assert.True(resultado.PasaPrueba, $"El generador falló la prueba de serie: {resultado.Mensaje}");
    }

    /// <summary>
    /// PRUEBA 7: Prueba de Kolmogorov-Smirnov (KS) sobre el Generador Mixto.
    /// OBJETIVO: Medir el grado de ajuste comparando la distribución acumulada empírica con la teórica uniforme.
    /// CÓMO SE PRUEBA: Se genera una muestra de 1000 números. Se calcula la mayor diferencia absoluta (Dn)
    /// entre la posición esperada del número ordenado (i/N) y su valor real.
    /// Se verifica que Dn sea inferior al valor límite crítico d_alfa,N = 0.043.
    /// </summary>
    [Fact]
    public void GeneradorMixto_DebePasar_PruebaKS()
    {
        // Arrange (Preparación)
        long m = 2147483648;
        long semilla = 123456789;
        long a = 1103515245;
        long c = 12345;
        int total = 1000; // N = 1000 es ideal para KS por ordenamiento eficiente
        
        double dAlfaN = 0.043; // Valor crítico de K-S para N=1000 y alfa=0.05 (calculado como 1.36 / raiz(1000))

        // Act (Ejecución)
        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).Skip(1).ToList();
        var resultado = _pruebasService.PruebaKS(numeros, dAlfaN);

        _output.WriteLine($"[K-S] Dn: {resultado.Dn}, D-Critico: {resultado.DAlfaN}, Pasa: {resultado.PasaPrueba}");

        // Assert (Verificación)
        Assert.True(resultado.PasaPrueba, $"El generador falló la prueba K-S: {resultado.Mensaje}");
    }

    /// <summary>
    /// PRUEBA 8: Prueba de Corridas (Runs) sobre el Generador Mixto.
    /// OBJETIVO: Evaluar la independencia física (ausencia de patrones de subida y bajada repetitivos).
    /// CÓMO SE PRUEBA: Se binariza la muestra según la mediana (0 si <= 0.5, 1 si > 0.5). Se agrupan los bits
    /// consecutivos idénticos en rachas o corridas. Se realiza un test Chi-Cuadrado de bondad de ajuste sobre la longitud
    /// de las corridas.
    /// Dado que el número máximo de intervalos (la longitud de corrida más larga) varía dinámicamente según la semilla,
    /// primero ejecutamos una pre-prueba rápida para encontrar la 'longitud máxima de corrida' y luego inyectamos
    /// dinámicamente el valor crítico de Chi-Cuadrado correcto obtenido de una tabla estática de referencia.
    /// </summary>
    [Fact]
    public void GeneradorMixto_DebePasar_PruebaCorridasMedia()
    {
        // Arrange (Preparación)
        long m = 2147483648;
        long semilla = 123456789;
        long a = 1103515245;
        long c = 12345;
        int total = 5000;
        
        // Ejecución preliminar (Pre-Act) para conocer la longitud de corrida máxima real
        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).Skip(1).ToList();
        var resultadoPre = _pruebasService.PruebaCorridasMedia(numeros, 999.0); // Le pasamos un alfa gigante para que no falle el pre-cálculo
        
        int longMaxima = resultadoPre.LongitudesCorridas.Max();
        
        // Obtenemos el valor de tabla crítico exacto para los grados de libertad de esta corrida (alfa = 0.05)
        double chiCuadradoAlfa = GetChiSquareCriticalValueAlfa05(longMaxima);

        // Act (Ejecución real) - Ejecutar la prueba con el límite de tabla correcto
        var resultado = _pruebasService.PruebaCorridasMedia(numeros, chiCuadradoAlfa);

        _output.WriteLine($"[Corridas] LongMaxima: {longMaxima}, Chi2: {resultado.ChiCuadrado}, Chi2-Critico: {resultado.ChiCuadradoAlfa}, Pasa: {resultado.PasaPrueba}");

        // Assert (Verificación)
        Assert.True(resultado.PasaPrueba, $"El generador falló la prueba de corridas: {resultado.Mensaje}. Chi2 calculado: {resultado.ChiCuadrado}, Crítico: {chiCuadradoAlfa}");
    }

    /// <summary>
    /// Función auxiliar estática que simula una tabla de distribución Chi-Cuadrado.
    /// Retorna el valor crítico para un nivel de significación del 5% (alfa = 0.05)
    /// correspondiente a los grados de libertad (GL) especificados por la longitud máxima de corrida.
    /// </summary>
    private double GetChiSquareCriticalValueAlfa05(int longMaxima)
    {
        // Grados de libertad = longitud máxima de corrida
        return longMaxima switch
        {
            1 => 3.841,
            2 => 5.991,
            3 => 7.815,
            4 => 9.488,
            5 => 11.070,
            6 => 12.592,
            7 => 14.067,
            8 => 15.507,
            9 => 16.919,
            10 => 18.307,
            11 => 19.675,
            12 => 21.026,
            13 => 22.362,
            14 => 23.685,
            15 => 24.996,
            _ => 31.410 // Valor genérico seguro para grados de libertad mayores a 15
        };
    }
}
