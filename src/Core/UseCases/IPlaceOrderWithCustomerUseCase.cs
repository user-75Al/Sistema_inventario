using System.Collections.Generic;
using System.Threading.Tasks;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Solicitud para realizar un pedido con múltiples productos y asociado a un cliente.
/// </summary>
public record OrderWithCustomerRequest(int CustomerId, List<OrderItemRequest> Items);

/// <summary>
/// Representa un item individual dentro de la solicitud de pedido.
/// </summary>
public record OrderItemRequest(int ProductId, int Quantity);

/// <summary>
/// Interfaz para el caso de uso de realizar un pedido asociado a un cliente.
/// </summary>
public interface IPlaceOrderWithCustomerUseCase
{
    Task<SaleResult> ExecuteAsync(OrderWithCustomerRequest request);
}
