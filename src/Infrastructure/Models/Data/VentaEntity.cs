using System;

namespace UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Mapeo 1:1 con la tabla [Venta].
/// Utiliza C# 14 'field' para replicar las restricciones CHECK de SQL Server.
/// </summary>
public partial class VentaEntity(int ventaId, string folio)
{
    public int VentaID { get; init; } = ventaId;
    public string Folio { get; init; } = folio;
    public int? ClienteID { get; set; }

    public DateTime FechaVenta { get; set; } = DateTime.Now;

    public int TotalArticulos
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(TotalArticulos));
    }

    public decimal TotalVenta
    {
        get => field;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(TotalVenta));
    }

    public byte Estatus
    {
        get => field;
        set => field = (value >= 1 && value <= 3) ? value : throw new ArgumentOutOfRangeException(nameof(Estatus), "Estatus inválido (1-3).");
    }

    public string? NombreCliente { get; set; }
}
