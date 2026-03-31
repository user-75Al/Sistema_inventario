using System.Collections.Generic;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Contrato para el caso de uso de detección de inventario crítico.
/// </summary>
public interface ILowStockAlertUseCase
{
    /// <summary>
    /// Identifica productos cuyo stock es igual o inferior al umbral definido.
    /// Utiliza streaming asíncrono para eficiencia en memoria.
    /// </summary>
    /// <param name="threshold">Cantidad máxima permitida para considerar stock bajo.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Flujo asíncrono de productos con bajo inventario.</returns>
    IAsyncEnumerable<Product> ExecuteAsync(int threshold, CancellationToken ct = default);
}
