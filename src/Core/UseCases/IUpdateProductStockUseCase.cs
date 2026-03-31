using System.Threading;
using System.Threading.Tasks;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Contract for atomic stock update operations.
/// </summary>
public interface IUpdateProductStockUseCase
{
    ValueTask ExecuteAsync(int id, int newStock, CancellationToken ct = default);
}
