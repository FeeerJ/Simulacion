using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ResultadoExtraccion
    {
        public List<ExtraccionUnidad> ExtraccionesCRT { get; set; }
        public List<ExtraccionUnidad> ExtraccionesLCD { get; set; }
        public List<ExtraccionUnidad> ExtraccionesLED { get; set; }
        public double TotalCobreKG { get; set; }
        public double TotalOroKG { get; set; }
        public double TotalPlataKG { get; set; }
        public double TotalPCBKg { get; set; }
    }
}
