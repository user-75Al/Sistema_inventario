using Microsoft.Extensions.DependencyInjection;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Data;
using UtmMarket.Infrastructure.Repositories;

namespace UtmMarket.Infrastructure;

/// <summary>
/// Contenedor de extensiones para el registro de servicios de infraestructura.
/// </summary>
public static class InfrastructureExtensions
{
    /// <summary>
    /// Registra los componentes de persistencia y factorías de conexión.
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddSingleton<DatabaseMigrator>();
        
        // Registro de repositorios
        services.AddScoped<IProductRepository, ProductRepositoryImpl>();
        services.AddScoped<ISaleRepository, SaleRepositoryImpl>();
        services.AddScoped<ICustomerRepository, CustomerRepositoryImpl>();
        
        return services;
    }
}
