using Simulacion.Domain.Entities;
using Simulacion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.PlantServices
{
    public class LlegadaService
    {
        private readonly IDistribution _distribuciones;

        public LlegadaService(IDistribution distribuciones)
        {
            _distribuciones = distribuciones;
        }

        public Camioneta ProcesarNuevaCamioneta()
        {
            double pesoTotal = _distribuciones.GenerarNormal(83, 15);
            /*TODAVIA NO ESTA ESTO */
            // double pesoAprovechable = _distribuciones.GenerarBinomial(pesoTotal, 0.90);
            // double pesoReventa = _distribuciones.GenerarBinomial(pesoAprovechable, 0.15);
            // double pesoDesmantelamiento = pesoAprovechable - pesoReventa;

            return new Camioneta
            {
                PesoTotal = pesoTotal,
               // PesoDescarte = pesoTotal - pesoAprovechable,
                //PesoReventa = pesoReventa,
                //PesoDesmantelamiento = pesoDesmantelamiento,
                Estado = "Recibido",
                FechaIngreso = DateTime.Now
            };
        }


    }



}
