using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Fetches all sales records as an asynchronous stream.
/// </summary>
public interface IFetchAllSalesUseCase
{
    IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken ct = default);
}
