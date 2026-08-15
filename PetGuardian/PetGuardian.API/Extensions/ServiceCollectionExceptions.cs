using Microsoft.EntityFrameworkCore;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Implementations;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Infrastructure.Persistence;
using PetGuardian.Infrastructure.Persistence.Repositories;

namespace PetGuardian.API.Extensions;

/// <summary>
/// Extensões para registrar persistência e repositórios da solução PetGuardian na injeção de dependências.
/// </summary>
public static class PetGuardianServiceCollectionExtensions
{
    /// <summary>
    /// Registra o <see cref="PetGuardianContext"/> com Oracle.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configuração (appsettings, variáveis de ambiente, etc.).</param>
    /// <param name="connectionStringName">Chave em ConnectionStrings para Oracle (padrão: PetGuardianOracle).</param>
    /// <returns>A mesma instância de <see cref="IServiceCollection"/> para encadeamento.</returns>
    /// <exception cref="InvalidOperationException">Quando a connection string não for encontrada.</exception>
    public static IServiceCollection AddPetGuardianDbContext(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = "PetGuardianOracle")
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{connectionStringName}' não encontrada.");

        services.AddDbContext<PetGuardianContext>(options =>
            options.UseOracle(connectionString, b =>
                b.UseOracleSQLCompatibility(Microsoft.EntityFrameworkCore.OracleSQLCompatibility.DatabaseVersion19)));

        return services;
    }

    /// <summary>
    /// Registra todas as implementações de repositório como <c>Scoped</c> (um por requisição HTTP).
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <returns>A mesma instância de <see cref="IServiceCollection"/> para encadeamento.</returns>
    public static IServiceCollection AddPetGuardianRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();
        services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
        services.AddScoped<IUsuarioPetRepository, UsuarioPetRepository>();
        services.AddScoped<IUsuarioEnderecoRepository, UsuarioEnderecoRepository>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    /// <summary>
    /// Adiciona serviços que orquestram repositórios.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <returns>A mesma instância de <see cref="IServiceCollection"/> para encadeamento.</returns>
    public static IServiceCollection AddPetGuardianApplicationServices(this IServiceCollection services)
    {
        // Hierarquia de endereço
        services.AddScoped<IEstadoService,   EstadoService>();
        services.AddScoped<ICidadeService,   CidadeService>();
        services.AddScoped<IBairroService,   BairroService>();
        services.AddScoped<IEnderecoService, EnderecoService>();

        // Lookup
        services.AddScoped<IRacaService,      RacaService>();
        services.AddScoped<IStatusService,    StatusService>();
        services.AddScoped<ITipoAtendService, TipoAtendService>();
        services.AddScoped<ITelefoneService,  TelefoneService>();

        // Core
        services.AddScoped<IClinicaService, ClinicaService>();
        services.AddScoped<IVeterinarioService, VeterinarioService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IPetService, PetService>();
        services.AddScoped<IAtendimentoService, AtendimentoService>();
        services.AddScoped<ITarefaService, TarefaService>();

        // Join tables
        services.AddScoped<IUsuarioPetService,UsuarioPetService>();
        services.AddScoped<IUsuarioEnderecoService, UsuarioEnderecoService>();

        return services;
    }
}
