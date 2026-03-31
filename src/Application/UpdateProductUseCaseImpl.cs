using System;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class UpdateProductUseCaseImpl(IProductRepository repository) : IUpdateProductUseCase
{
    public async ValueTask ExecuteAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        var existing = await repository.GetByIdAsync(product.ProductID, ct: ct);
        if (existing is null)
            throw new InvalidOperationException($"Product with ID {product.ProductID} not found.");

        await repository.UpdateAsync(product, ct: ct);
    }
}
