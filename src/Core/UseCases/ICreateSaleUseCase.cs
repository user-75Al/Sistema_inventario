using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Orchestrates the logic for creating and persisting a new sale transaction.
/// </summary>
public interface ICreateSaleUseCase
{
    ValueTask<Sale> ExecuteAsync(Sale sale, CancellationToken ct = default);
}
