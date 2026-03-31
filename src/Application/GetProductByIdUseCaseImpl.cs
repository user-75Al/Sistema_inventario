using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class GetProductByIdUseCaseImpl(IProductRepository repository) : IGetProductByIdUseCase
{
    public async ValueTask<Product?> ExecuteAsync(int id, CancellationToken ct = default) =>
        await repository.GetByIdAsync(id, ct: ct);
}
