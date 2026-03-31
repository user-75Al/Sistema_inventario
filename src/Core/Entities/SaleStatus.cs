namespace UtmMarket.Core.Entities;

/// <summary>
/// Representa los posibles estados de una venta en el dominio.
/// </summary>
public enum SaleStatus : byte
{
    Pending = 1,
    Completed = 2,
    Canceled = 3
}
