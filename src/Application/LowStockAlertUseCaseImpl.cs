using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación eficiente del caso de uso de alerta de bajo inventario.
/// Diseñado para streaming asíncrono y compatibilidad con Native AOT.
/// </summary>
public class LowStockAlertUseCaseImpl(IProductRepository productRepository) : ILowStockAlertUseCase
{
    /// <summary>
    /// Ejecuta el filtrado de productos mediante streaming asíncrono.
    /// No materializa colecciones en memoria, permitiendo procesar grandes volúmenes de datos con eficiencia.
    /// </summary>
    public async IAsyncEnumerable<Product> ExecuteAsync(int threshold, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        // El flujo de datos se procesa elemento por elemento desde el origen (repositorio) hasta el consumidor final.
        await foreach (var product in productRepository.GetAllAsync(ct))
        {
            if (product.Stock <= threshold)
            {
                yield return product;
            }
        }
    }
}
