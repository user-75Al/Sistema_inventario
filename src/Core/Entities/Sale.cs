using System;
using System.Collections.Generic;
using System.Linq;

namespace UtmMarket.Core.Entities;

/// <summary>
/// Agregado raíz que representa una transacción de venta.
/// </summary>
public class Sale(int saleId, string folio)
{
    public int SaleID { get; init; } = saleId;
    public string Folio { get; init; } = folio;
    public DateTime SaleDate { get; init; } = DateTime.Now;
    public SaleStatus Status { get; set; } = SaleStatus.Pending;
    public int? CustomerID { get; set; }
    public string? CustomerName { get; set; }

    /// <summary>
    /// Colección de detalles de la venta.
    /// </summary>
    public List<SaleDetail> Details { get; init; } = [];

    /// <summary>
    /// Suma total de artículos en la venta (dinámica).
    /// </summary>
    public int TotalItems => Details.Sum(d => d.Quantity);

    /// <summary>
    /// Monto total de la venta calculado dinámicamente.
    /// </summary>
    public decimal TotalSale => Details.Sum(d => d.TotalDetail);

    /// <summary>
    /// Lógica de negocio para añadir un producto a la venta.
    /// </summary>
    public void AddDetail(Product product, int quantity)
    {
        if (product.Stock < quantity)
            throw new InvalidOperationException($"Stock insuficiente para el producto: {product.Name}");

        Details.Add(new SaleDetail(product, quantity));
    }
}
