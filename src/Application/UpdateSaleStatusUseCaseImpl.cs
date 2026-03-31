using System;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class UpdateSaleStatusUseCaseImpl(ISaleRepository repository) : IUpdateSaleStatusUseCase
{
    public async ValueTask ExecuteAsync(int id, SaleStatus status, CancellationToken ct = default)
    {
        var sale = await repository.GetByIdAsync(id, cancellationToken: ct);
        if (sale is null) throw new InvalidOperationException($"Sale {id} not found.");

        sale.Status = status;
        await repository.UpdateAsync(sale, cancellationToken: ct);
    }
}
