using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Updates exclusively the status of an existing sale.
/// </summary>
public interface IUpdateSaleStatusUseCase
{
    ValueTask ExecuteAsync(int id, SaleStatus status, CancellationToken ct = default);
}
