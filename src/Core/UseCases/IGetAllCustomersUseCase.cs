using System.Collections.Generic;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Caso de uso para recuperar todos los clientes registrados.
/// </summary>
public interface IGetAllCustomersUseCase
{
    /// <summary>
    /// Ejecuta el flujo asíncrono para obtener clientes.
    /// </summary>
    IAsyncEnumerable<Customer> ExecuteAsync(CancellationToken ct = default);
}
