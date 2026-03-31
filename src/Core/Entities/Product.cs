namespace UtmMarket.Core.Entities;

/// <summary>
/// Entidad pura de dominio que representa un producto en el catálogo.
/// Utiliza Primary Constructors y C# 14 'field' para validación compacta.
/// </summary>
public class Product(int productId, string name, string sku, string brand, decimal price, int stock)
{
    public int ProductID { get; init; } = productId;
    public string Name { get; set; } = name;
    public string SKU { get; set; } = sku;
    public string Brand { get; set; } = brand;

    public decimal Price
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentException("El precio no puede ser negativo.");
    } = price;

    public int Stock
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentException("El stock no puede ser negativo.");
    } = stock;
}
