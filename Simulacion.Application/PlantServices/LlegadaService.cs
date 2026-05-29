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

        /*
        public Device ProcesarNuevaCamioneta()
        {
            var lote = new Device();
            /*Generamos el Volumen que trae*/
           // double pesoTotal = _distribuciones.GenerarNormal(83, 15);

            /*Evaluamos si es apto*/
           
        //}*/


    }



}
