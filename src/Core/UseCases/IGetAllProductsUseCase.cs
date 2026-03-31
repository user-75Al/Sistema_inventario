using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Contract for retrieving all products in the catalog.
/// </summary>
public interface IGetAllProductsUseCase
{
    IAsyncEnumerable<Product> ExecuteAsync(CancellationToken ct = default);
}
