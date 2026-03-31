using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Contract for updating the information of an existing product.
/// </summary>
public interface IUpdateProductUseCase
{
    ValueTask ExecuteAsync(Product product, CancellationToken ct = default);
}
