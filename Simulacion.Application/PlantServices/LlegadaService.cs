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
        private int _totalDispositivos = 0;
        private int _totalCamionetas = 0;
        public LlegadaService(IDistribution distribuciones)
        {
            _distribuciones = distribuciones;
        }

        public Camioneta ProcesarNuevaCamioneta()
        {
            int CantDispositivos = (int)_distribuciones.GenerarNormal(50, 10);
            _totalDispositivos += CantDispositivos;
            _totalCamionetas += 1;

            return new Camioneta
            {
                CantDispositivos = CantDispositivos,
                Estado = "Recibido",
               
            };
        }

        public ResumenLlegada ObtenerResumen()
        {
            return new ResumenLlegada
            {
                TotalCamiones = _totalCamionetas,
                TotalDispositivos = _totalDispositivos,
                PromedioDispositivosPorCamion = _totalCamionetas > 0 ? (double)_totalDispositivos / _totalCamionetas : 0
            };

        }


    }



}
