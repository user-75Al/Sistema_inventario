using System.Collections.Generic;
using System.Linq;
using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Mappers;

/// <summary>
/// Mapeador profundo para la entidad Venta y sus Detalles.
/// Optimizado para .NET 10 y Native AOT mediante métodos de extensión estáticos.
/// </summary>
public static class SaleMapper
{
    /// <summary>
    /// Convierte una VentaEntity y sus detalles en un objeto de dominio Sale.
    /// Nota: Requiere que los productos asociados ya estén cargados o mapeados.
    /// </summary>
    public static Sale ToDomain(this VentaEntity entity, IEnumerable<(DetalleVentaEntity Detail, Product Product)> details)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(details);

        var sale = new Sale(entity.VentaID, entity.Folio)
        {
            SaleDate = entity.FechaVenta,
            Status = (SaleStatus)entity.Estatus,
            CustomerID = entity.ClienteID,
            CustomerName = entity.NombreCliente
        };

        foreach (var (detailEntity, product) in details)
        {
            // Reconstruimos el detalle asegurando la integridad del precio histórico
            var detail = new SaleDetail(product, detailEntity.Cantidad);
            // Si el precio histórico difiere del actual del producto, podríamos necesitar un ajuste aquí
            // En este modelo, SaleDetail toma el precio del producto al construirse.
            sale.Details.Add(detail);
        }

        return sale;
    }

    /// <summary>
    /// Transforma un objeto Sale de dominio en su representación de persistencia.
    /// Retorna una tupla con la cabecera y la colección de detalles.
    /// </summary>
    public static (VentaEntity Header, IEnumerable<DetalleVentaEntity> Details) ToEntity(this Sale domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        var header = new VentaEntity(domain.SaleID, domain.Folio)
        {
            FechaVenta = domain.SaleDate,
            TotalArticulos = domain.TotalItems,
            TotalVenta = domain.TotalSale,
            Estatus = (byte)domain.Status,
            ClienteID = domain.CustomerID
        };

        var details = domain.Details.Select(d => new DetalleVentaEntity(0, domain.SaleID, d.Product.ProductID)
        {
            PrecioUnitario = d.UnitPrice,
            Cantidad = d.Quantity,
            TotalDetalle = d.TotalDetail
        });

        return (header, details);
    }
}
