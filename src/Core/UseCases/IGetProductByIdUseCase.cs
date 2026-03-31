using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Contract for retrieving a single product by its unique identifier.
/// </summary>
public interface IGetProductByIdUseCase
{
    ValueTask<Product?> ExecuteAsync(int id, CancellationToken ct = default);
}
