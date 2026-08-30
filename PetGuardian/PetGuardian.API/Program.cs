using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PetGuardian.API.Exceptions;
using PetGuardian.API.Extensions;
using Serilog;

namespace PetGuardian.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ----- SPRINT 3: Logging estruturado com Serilog -----
        // Console + arquivo (rolling diário), níveis padrão de Information/Warning/Error,
        // enriquecido com CorrelationId via CorrelationIdMiddleware (ver Extensions/ObservabilityExtensions.cs).
        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "PetGuardian.API")
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "logs/petguardian-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ({CorrelationId}) {SourceContext}: {Message:lj}{NewLine}{Exception}"));

        builder.Services.AddPetGuardianDbContext(builder.Configuration);
        builder.Services.AddPetGuardianRepositories();
        builder.Services.AddPetGuardianApplicationServices();
        builder.Services.AddPetGuardianObservability(builder.Configuration); // SPRINT 3
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PetGuardian API",
                Version = "v1",
                Description = "API REST para gerenciamento da rede de cuidado colaborativo de pets.",
                Contact = new OpenApiContact
                {
                    Name = "Equipe PetGuardian",
                    Email = "contato@petguardian.com"
                }
            });
            var apiXml = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXml);
            if (File.Exists(apiXmlPath))
            {
                options.IncludeXmlComments(apiXmlPath);
            }
            var appXml = "PetGuardian.Application.xml";
            var appXmlPath = Path.Combine(AppContext.BaseDirectory, appXml);
            if (File.Exists(appXmlPath))
            {
                options.IncludeXmlComments(appXmlPath);
            }
        });

        var app = builder.Build();

        app.UseSerilogRequestLogging(); // loga método, rota, status e duração de cada requisição

        app.UseExceptionHandler();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetGuardian API v1");
            options.RoutePrefix = string.Empty;
        });
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UsePetGuardianObservability(); // SPRINT 3: /health, /health/ready, /health/live + middlewares

        app.MapControllers();
        app.Run();
    }
}