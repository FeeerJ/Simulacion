using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Domain.Entities
{
    public class ExtraccionUnidad
    {
        public double CobreG { get; set; }   // Solo CRT
        public double PCBKg { get; set; }    // Solo LCD/LED
        public double OroG { get; set; }     // Solo LCD/LED
        public double PlataG { get; set; }   // Solo LCD/LED
    }
}
