namespace UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Mapeo 1:1 con la tabla [DetalleVenta].
/// Diseñado para persistencia de alto rendimiento sin sobrecarga de ORM.
/// </summary>
public partial class DetalleVentaEntity(int detalleId, int ventaId, int productoId)
{
    public int DetalleID { get; init; } = detalleId;
    public int VentaID { get; init; } = ventaId;
    public int ProductoID { get; init; } = productoId;

    public decimal PrecioUnitario
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(PrecioUnitario));
    }

    public int Cantidad
    {
        get => field;
        set => field = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(Cantidad), "La cantidad debe ser mayor a cero.");
    }

    public decimal TotalDetalle
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(TotalDetalle));
    }
}
