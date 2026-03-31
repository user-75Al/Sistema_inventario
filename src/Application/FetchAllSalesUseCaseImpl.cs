using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class FetchAllSalesUseCaseImpl(ISaleRepository repository) : IFetchAllSalesUseCase
{
    public IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken ct = default) =>
        repository.GetAllAsync(ct);
}
