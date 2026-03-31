namespace UtmMarket.Core.Entities;

/// <summary>
/// Representa el desglose de un producto dentro de una venta.
/// Captura el precio histórico en el momento de la transacción.
/// </summary>
public class SaleDetail(Product product, int quantity)
{
    public Product Product { get; init; } = product ?? throw new ArgumentNullException(nameof(product));
    
    public decimal UnitPrice { get; init; } = product.Price;

    public int Quantity 
    { 
        get => field; 
        set => field = value > 0 ? value : throw new ArgumentException("La cantidad debe ser mayor a cero."); 
    } = quantity;

    /// <summary>
    /// Propiedad calculada para el subtotal del detalle.
    /// </summary>
    public decimal TotalDetail => UnitPrice * Quantity;
}
