
using Simulacion.Application.PlantServices;
using Simulacion.Application.Services;
using Simulacion.Domain.Interfaces;
using Simulacion.Exceptions;

namespace Simulacion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            /*Configuracion de CORS*/
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddScoped<MidSquareService>();
            builder.Services.AddScoped<LehmerService>();
            builder.Services.AddScoped<CongruentialMethodService>();
            /**/
            builder.Services.AddSingleton<IDistribution>(provider =>
            {
                var generador = new CongruentialMethodService();
                var semilla = DateTime.Now.Ticks % 100000; // Generar una semilla basada en el tiempo actual, se toma el modulo de 100000 para limitar su tamaño
                var listaU = generador.GenerateMixed(100000, semilla, 1021, 3, 99000);
                return new DistributionService(listaU);
            });
            builder.Services.AddSingleton<LlegadaService>();
            builder.Services.AddSingleton<SegmentacionService>();
            builder.Services.AddScoped<SimulacionService>();
            builder.Services.AddSingleton<PesoService>();
            builder.Services.AddSingleton<SustanciasToxicasService>();
            builder.Services.AddSingleton<ExtraccionMaterialesService>();
            /**/
            builder.Services.AddScoped<StatisticalTestsService>();

            /*Servicios para el Middleware*/
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
