using Xunit;
using System.Collections.Generic;
using System.Linq;
using Simulacion.Application.Services;
using static Simulacion.Application.Dtos.PruebasEstadisticasRequesDto;

namespace Simulacion.Tests;

public class GeneradoresYPruebasEstadisticasTests
{
    private readonly CongruentialMethodService _generador;
    private readonly StatisticalTestsService _pruebasService;

    public GeneradoresYPruebasEstadisticasTests()
    {

        _generador = new CongruentialMethodService();
        _pruebasService = new StatisticalTestsService();
    }

    [Fact]
    public void GeneradorMixto_DebeGenerarValoresEnRangoCeroAUno()
    {

        long m = 100000;
        long semilla = 1021;
        long a = 3;
        long c = 99000;
        int total = 1000;


        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).ToList();


        Assert.Equal(total + 1, numeros.Count);
        Assert.All(numeros, n => Assert.True(n >= 0.0 && n < 1.0, $"El número {n} está fuera de los límites [0, 1)"));
    }

    [Fact]
    public void PruebaPromedios_ConDistribuciónUniformePerfecta_DebePasarLaPrueba()
    {


        var numerosDistribuidos = new List<double> { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };
        double zAlfa = 1.96;


        PruebaPromediosResultDto resultado = _pruebasService.PruebaPromedios(numerosDistribuidos, zAlfa);


        Assert.True(resultado.PasaPrueba, $"La prueba de promedios falló inesperadamente: {resultado.Mensaje}");
        Assert.Equal(0.5, resultado.Promedio);
        Assert.Equal(0.0, resultado.Z0);
    }

    [Fact]
    public void PruebaPromedios_ConValoresSesgados_DebeRechazarLaHipotesis()
    {


        var numerosSesgados = new List<double> { 0.01, 0.02, 0.05, 0.03, 0.01, 0.04, 0.02 };
        double zAlfa = 1.96;


        PruebaPromediosResultDto resultado = _pruebasService.PruebaPromedios(numerosSesgados, zAlfa);


        Assert.False(resultado.PasaPrueba, "Se esperaba que la prueba fallara debido al fuerte sesgo.");
    }

    [Fact]
    public void Integracion_GeneradorMixto_DebePasarPruebaFrecuencias()
    {


        long m = 16777216;
        long semilla = 123456;
        long a = 1103515245;
        long c = 12345;
        int total = 1000;

        var numeros = _generador.GenerateMixed(m, semilla, a, c, total).ToList();

        int subintervalos = 10;
        double chiCuadradoAlfa = 16.919;


        PruebaFrecuenciaResultDto resultado = _pruebasService.PruebaFrecuencia(numeros, subintervalos, chiCuadradoAlfa);




        Assert.True(resultado.PasaPrueba, $"El generador mixto no superó la prueba de frecuencia: {resultado.Mensaje} (Chi2 calculado: {resultado.ChiCuadrado})");
    }
}
