using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Caso de uso para el registro de nuevos clientes en el sistema.
/// </summary>
public interface ICreateCustomerUseCase
{
    /// <summary>
    /// Ejecuta la lógica de negocio para registrar un cliente.
    /// Valida que el email no esté duplicado antes de proceder.
    /// </summary>
    Task<int> ExecuteAsync(Customer customer, CancellationToken ct = default);
}
