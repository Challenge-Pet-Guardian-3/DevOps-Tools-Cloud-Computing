using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PetGuardian.API.Exceptions;
using PetGuardian.API.Extensions;
using PetGuardian.Infrastructure.Persistence;

namespace PetGuardian.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddPetGuardianDbContext(builder.Configuration);
        builder.Services.AddPetGuardianRepositories();
        builder.Services.AddPetGuardianApplicationServices();

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

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PetGuardianContext>();
            dbContext.Database.Migrate();
        }

        app.UseExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetGuardian API v1");
            options.RoutePrefix = string.Empty;
        });

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
