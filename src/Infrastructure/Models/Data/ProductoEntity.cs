namespace UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Mapeo 1:1 con la tabla [Producto].
/// Optimizado para Native AOT mediante clases parciales y sin reflexión.
/// </summary>
public partial class ProductoEntity(int productoId, string sku)
{
    public int ProductoID { get; init; } = productoId;
    public string SKU { get; init; } = sku;
    
    public string? Nombre { get; set; }
    public string? Marca { get; set; }

    public decimal Precio
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(Precio), "El precio no puede ser negativo.");
    }

    public int Stock
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(Stock), "El stock no puede ser negativo.");
    }
}
