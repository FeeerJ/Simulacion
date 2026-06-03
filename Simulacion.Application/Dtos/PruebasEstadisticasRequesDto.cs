using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.Dtos;

public class PruebasEstadisticasRequesDto
{
    public class PromediosRequestDto
    {
        public List<double> Numeros { get; set; } = new List<double>();
        public double ZAlfa { get; set; }
    }
    public class PruebaPromediosResultDto
    {
        public double Promedio { get; set; }
        public double Z0 { get; set; }
        public double ZAlfa { get; set; }
        public bool PasaPrueba { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }

    public class FrecuenciaRequestDto
    {
        public List<double> Numeros { get; set; } = new List<double>();
        public int CantidadSubintervalos { get; set; } // Representa la variable 'x' del PDF
        public double ChiCuadradoAlfa { get; set; }
    }
    public class PruebaFrecuenciaResultDto
    {
        public List<int> FrecuenciasObservadas { get; set; } = new List<int>();
        public double FrecuenciaEsperada { get; set; }
        public double ChiCuadrado { get; set; }
        public double ChiCuadradoAlfa { get; set; }
        public bool PasaPrueba { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }

    public class SerieRequestDto
    {
        public List<double> Numeros { get; set; } = new List<double>();
        public int X { get; set; } // El número de subdivisiones del lado del cuadrado unitario
        public double ChiCuadradoAlfa { get; set; }
    }
    public class PruebaSerieResultDto
    {
        public int[][] FrecuenciasObservadas { get; set; } = Array.Empty<int[]>();
        public double FrecuenciaEsperada { get; set; }
        public double ChiCuadrado { get; set; }
        public double ChiCuadradoAlfa { get; set; }
        public bool PasaPrueba { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
    public class KSRequestDto
    {
        public List<double> Numeros { get; set; } = new List<double>();
        public double DAlfaN { get; set; } // Estadístico d alfa,n
    }
    public class PruebaKSResultDto
    {
        public List<double> NumerosOrdenados { get; set; } = new List<double>();
        public List<double> DistribucionAcumulada { get; set; } = new List<double>();
        public List<double> Diferencias { get; set; } = new List<double>();
        public double Dn { get; set; }
        public double DAlfaN { get; set; }
        public bool PasaPrueba { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
    public class CorridasRequestDto
    {
        public List<double> Numeros { get; set; } = new List<double>();
        public double ChiCuadradoAlfa { get; set; }
    }

    public class PruebaCorridasResultDto
    {
        public string SecuenciaBinaria { get; set; } = string.Empty;
        public List<int> LongitudesCorridas { get; set; } = new List<int>();
        public List<int> FrecuenciasObservadas { get; set; } = new List<int>();
        public List<double> FrecuenciasEsperadas { get; set; } = new List<double>();
        public double ChiCuadrado { get; set; }
        public double ChiCuadradoAlfa { get; set; }
        public bool PasaPrueba { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
