using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class FetchSaleByIdUseCaseImpl(ISaleRepository repository) : IFetchSaleByIdUseCase
{
    public async ValueTask<Sale?> ExecuteAsync(int id, CancellationToken ct = default) =>
        await repository.GetByIdAsync(id, cancellationToken: ct);
}
