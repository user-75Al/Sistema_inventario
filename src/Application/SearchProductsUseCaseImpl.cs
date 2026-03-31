using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class SearchProductsUseCaseImpl(IProductRepository repository) : ISearchProductsUseCase
{
    public IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken ct = default) =>
        repository.FindAsync(filter, ct);
}
