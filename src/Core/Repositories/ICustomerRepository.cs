using UtmMarket.Core.Entities;

namespace UtmMarket.Core.Repositories;

/// <summary>
/// Contrato de persistencia para la gestión de clientes.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Recupera un cliente por su dirección de correo electrónico.
    /// </summary>
    Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Recupera un cliente por su identificador único.
    /// </summary>
    Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Recupera todos los clientes mediante streaming asíncrono.
    /// </summary>
    IAsyncEnumerable<Customer> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Registra un nuevo cliente en el sistema.
    /// </summary>
    Task<int> AddAsync(Customer customer, CancellationToken ct = default);

    /// <summary>
    /// Elimina un cliente del sistema por su identificador.
    /// </summary>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
