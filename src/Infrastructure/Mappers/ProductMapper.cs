using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Mappers;

/// <summary>
/// Mapeador estático ultra-eficiente para la entidad Producto.
/// Diseñado para compatibilidad total con Native AOT y Zero Reflection.
/// </summary>
public static class ProductMapper
{
    /// <summary>
    /// Transforma una entidad de persistencia en un objeto de dominio.
    /// </summary>
    /// <param name="entity">Entidad proveniente de la base de datos.</param>
    /// <returns>Objeto de dominio Product.</returns>
    public static Product ToDomain(this ProductoEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new Product(
            productId: entity.ProductoID,
            name: entity.Nombre ?? string.Empty,
            sku: entity.SKU,
            brand: entity.Marca ?? string.Empty,
            price: entity.Precio,
            stock: entity.Stock
        );
    }

    /// <summary>
    /// Transforma un objeto de dominio en una entidad de persistencia.
    /// </summary>
    /// <param name="domain">Objeto de dominio.</param>
    /// <returns>Entidad para persistencia en base de datos.</returns>
    public static ProductoEntity ToEntity(this Product domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return new ProductoEntity(domain.ProductID, domain.SKU)
        {
            Nombre = domain.Name,
            Marca = domain.Brand,
            Precio = domain.Price,
            Stock = domain.Stock
        };
    }
}
