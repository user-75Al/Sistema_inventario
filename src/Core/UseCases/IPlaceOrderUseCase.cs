using System.Threading.Tasks;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Solicitud para realizar un pedido de un solo producto.
/// </summary>
public record OrderRequest(int ProductId, int Quantity);

/// <summary>
/// Resultado del proceso de realizar un pedido.
/// </summary>
public record SaleResult(int SaleId, string Folio, decimal Total, bool Success, string Message);

/// <summary>
/// Interfaz para el caso de uso de realizar un pedido (venta).
/// </summary>
public interface IPlaceOrderUseCase
{
    Task<SaleResult> ExecuteAsync(OrderRequest request);
}
