using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Contract for registering a new product in the system.
/// </summary>
public interface ICreateProductUseCase
{
    ValueTask<int> ExecuteAsync(Product product, CancellationToken ct = default);
}
