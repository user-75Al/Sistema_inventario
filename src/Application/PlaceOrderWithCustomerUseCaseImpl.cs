using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.Infrastructure.Data;

namespace UtmMarket.Application;

/// <summary>
/// Implementación transaccional del caso de uso para realizar un pedido con cliente.
/// Garantiza que la venta, sus detalles y la actualización de stock sean atómicos.
/// </summary>
public class PlaceOrderWithCustomerUseCaseImpl(
    IProductRepository productRepository, 
    ISaleRepository saleRepository,
    ICustomerRepository customerRepository,
    IDbConnectionFactory connectionFactory) : IPlaceOrderWithCustomerUseCase
{
    public async Task<SaleResult> ExecuteAsync(OrderWithCustomerRequest request)
    {
        // 1. Validaciones iniciales
        if (request.Items == null || request.Items.Count == 0)
        {
            return new SaleResult(0, string.Empty, 0, false, "El pedido no contiene productos.");
        }

        // Abrimos la conexión manualmente para gestionar la transacción a nivel de Aplicación
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync();
        SqlTransaction? transaction = null;

        try
        {
            // Iniciamos la transacción
            transaction = connection.BeginTransaction();

            // 2. Validar que el cliente existe
            var customer = await customerRepository.GetByIdAsync(request.CustomerId);

            if (customer == null)
            {
                return new SaleResult(0, string.Empty, 0, false, "El cliente no existe en la base de datos.");
            }

            // 3. Generar Folio
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string folio = $"V-{timestamp.Substring(timestamp.Length - 10)}";

            // 4. Crear el objeto de dominio Sale
            var sale = new Sale(0, folio)
            {
                CustomerID = request.CustomerId,
                Status = SaleStatus.Completed // Asumimos completada al momento de la venta
            };

            // 5. Procesar cada item
            foreach (var item in request.Items)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId, transaction);
                if (product == null)
                {
                    throw new InvalidOperationException($"Producto con ID {item.ProductId} no encontrado.");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Stock insuficiente para {product.Name}. Disponible: {product.Stock}");
                }

                // Añadir al dominio (esto valida stock también internamente)
                sale.AddDetail(product, item.Quantity);

                // Actualizar stock atómicamente
                int newStock = product.Stock - item.Quantity;
                await productRepository.UpdateStockAsync(product.ProductID, newStock, transaction);
            }

            // 6. Persistir la venta y detalles
            var registeredSale = await saleRepository.AddAsync(sale, transaction);

            // 7. Commit
            await transaction.CommitAsync();

            return new SaleResult(
                registeredSale.SaleID, 
                registeredSale.Folio, 
                registeredSale.TotalSale, 
                true, 
                "Venta con cliente registrada exitosamente.");
        }
        catch (Exception ex)
        {
            if (transaction != null) await transaction.RollbackAsync();
            return new SaleResult(0, string.Empty, 0, false, $"Error en la transacción: {ex.Message}");
        }
    }
}
