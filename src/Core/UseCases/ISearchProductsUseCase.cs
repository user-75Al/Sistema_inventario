using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Contract for searching products using dynamic criteria (filters).
/// </summary>
public interface ISearchProductsUseCase
{
    IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken ct = default);
}
