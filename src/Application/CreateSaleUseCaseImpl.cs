using System;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class CreateSaleUseCaseImpl(ISaleRepository saleRepository, IProductRepository productRepository) : ICreateSaleUseCase
{
    public async ValueTask<Sale> ExecuteAsync(Sale sale, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sale);

        if (sale.Details.Count == 0)
            throw new InvalidOperationException("Cannot create a sale without details.");

        // Orquestación: Verificar y descontar stock
        foreach (var detail in sale.Details)
        {
            var product = await productRepository.GetByIdAsync(detail.Product.ProductID, ct: ct);
            if (product is null) throw new InvalidOperationException($"Product {detail.Product.ProductID} not found.");
            
            if (product.Stock < detail.Quantity)
                throw new InvalidOperationException($"Insufficient stock for {product.Name}. Available: {product.Stock}");

            await productRepository.UpdateStockAsync(product.ProductID, product.Stock - detail.Quantity, ct: ct);
        }

        return await saleRepository.AddAsync(sale, cancellationToken: ct);
    }
}
