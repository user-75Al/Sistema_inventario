using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.Infrastructure.Data;

namespace UtmMarket.Application;

/// <summary>
/// Implementación del caso de uso para realizar un pedido de forma transaccional.
/// </summary>
public class PlaceOrderUseCaseImpl(
    IProductRepository productRepository, 
    ISaleRepository saleRepository,
    IDbConnectionFactory connectionFactory) : IPlaceOrderUseCase
{
    public async Task<SaleResult> ExecuteAsync(OrderRequest request)
    {
        // Abrimos la conexión manualmente para gestionar la transacción a nivel de Aplicación
        var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync();
        SqlTransaction? transaction = null;

        try
        {
            // Iniciamos la transacción (la conexión ya está abierta por el factory)
            transaction = connection.BeginTransaction();

            // 1. Obtener el producto por ID dentro de la transacción
            var product = await productRepository.GetByIdAsync(request.ProductId, transaction);
            
            // 2. Validar existencia y stock suficiente
            if (product == null)
            {
                return new SaleResult(0, string.Empty, 0, false, "Producto no encontrado.");
            }

            if (product.Stock < request.Quantity)
            {
                return new SaleResult(0, string.Empty, 0, false, $"Stock insuficiente. Disponible: {product.Stock}");
            }

            // 3. Generar un folio único (Máximo 20 caracteres)
            // Ejemplo: V-63907806124398 -> sigue siendo largo.
            // Usaremos una parte de los Ticks y el ID del producto o un prefijo más corto.
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string folio = $"V-{timestamp.Substring(timestamp.Length - 10)}"; // "V-" + 10 dígitos = 12 caracteres

            // 4. Crear registro de venta y detalle
            var sale = new Sale(0, folio);
            sale.AddDetail(product, request.Quantity);

            // 5. Registrar la venta (cabecera y detalle)
            // Nota: El repositorio usará la transacción inyectada
            var registeredSale = await saleRepository.AddAsync(sale, transaction);

            // 6. Actualizar stock del producto de forma atómica
            int newStock = product.Stock - request.Quantity;
            await productRepository.UpdateStockAsync(product.ProductID, newStock, transaction);

            // 7. Commit si todo fue exitoso
            await transaction.CommitAsync();

            return new SaleResult(
                registeredSale.SaleID, 
                registeredSale.Folio, 
                registeredSale.TotalSale, 
                true, 
                "Venta realizada exitosamente.");
        }
        catch (Exception ex)
        {
            // Rollback en caso de error si la transacción fue iniciada
            if (transaction != null) await transaction.RollbackAsync();
            return new SaleResult(0, string.Empty, 0, false, $"Error al realizar la venta: {ex.Message}");
        }
        finally
        {
            // Siempre liberar recursos
            if (transaction != null) await transaction.DisposeAsync();
            await connection.DisposeAsync();
        }
    }
}
