using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class DeleteProductUseCaseImpl(IProductRepository productRepository) : IDeleteProductUseCase
{
    public Task ExecuteAsync(int id, CancellationToken ct = default)
    {
        return productRepository.DeleteAsync(id, ct: ct);
    }
}
