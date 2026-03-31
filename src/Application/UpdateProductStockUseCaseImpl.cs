using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class UpdateProductStockUseCaseImpl(IProductRepository repository) : IUpdateProductStockUseCase
{
    public async ValueTask ExecuteAsync(int id, int newStock, CancellationToken ct = default)
    {
        if (newStock < 0)
            throw new ArgumentOutOfRangeException(nameof(newStock), "Stock cannot be negative.");

        await repository.UpdateStockAsync(id, newStock, ct: ct);
    }
}
