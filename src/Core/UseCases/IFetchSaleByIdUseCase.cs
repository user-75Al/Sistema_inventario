using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Retrieves a specific sale by its unique identifier.
/// </summary>
public interface IFetchSaleByIdUseCase
{
    ValueTask<Sale?> ExecuteAsync(int id, CancellationToken ct = default);
}
