using System;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class CreateProductUseCaseImpl(IProductRepository repository) : ICreateProductUseCase
{
    public async ValueTask<int> ExecuteAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        // Validación de negocio: SKU no duplicado (se asume validación en repo o service extra)
        return await repository.AddAsync(product, ct: ct);
    }
}
