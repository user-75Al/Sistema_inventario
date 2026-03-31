using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class FetchSalesByFilterUseCaseImpl(ISaleRepository repository) : IFetchSalesByFilterUseCase
{
    public async Task<IEnumerable<Sale>> ExecuteAsync(SaleFilter filter, CancellationToken ct = default)
    {
        var sales = new List<Sale>();
        await foreach (var sale in repository.FindAsync(filter, ct))
        {
            sales.Add(sale);
        }
        return sales;
    }
}
