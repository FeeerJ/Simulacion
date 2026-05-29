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
        private readonly IGenerator _generador;

        public LlegadaService (IGenerator generador)
        {
            _generador = generador;
        }
    }


    /*Meotdo que se va a llaamr cuando llegue un camion*//*
    private List<Device> RecibirCarga(double relojGlobal)
        {
            var cargaCamion = new List<Device>();

            /*Generamos el peso total del camion*/
            //double pesoTotal = _generador.
        //}
    }
