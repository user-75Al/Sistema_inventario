using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Fetches sales records based on provided domain criteria.
/// </summary>
public interface IFetchSalesByFilterUseCase
{
    Task<IEnumerable<Sale>> ExecuteAsync(SaleFilter filter, CancellationToken ct = default);
}
