using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.Repositories;

/// <summary>
/// Criterios de búsqueda para productos. 
/// Diseñado como 'record' para inmutabilidad y compatibilidad AOT.
/// </summary>
public record ProductFilter(
    string? Name = null, 
    string? Brand = null, 
    decimal? MinPrice = null, 
    decimal? MaxPrice = null);

/// <summary>
/// Define el contrato de persistencia para la entidad Product.
/// Sigue principios de Clean Architecture y está optimizado para .NET 10.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Recupera todos los productos mediante streaming asíncrono.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Flujo asíncrono de objetos de dominio Product.</returns>
    IAsyncEnumerable<Product> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Busca un producto por su identificador único.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <param name="transaction">Optional transaction context.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>El producto si existe, de lo contrario null.</returns>
    Task<Product?> GetByIdAsync(int id, IDbTransaction? transaction = null, CancellationToken ct = default);

    /// <summary>
    /// Realiza una búsqueda filtrada de productos sin usar expresiones dinámicas.
    /// </summary>
    /// <param name="filter">Objeto con los criterios de filtrado.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Flujo asíncrono de productos que coinciden con los criterios.</returns>
    IAsyncEnumerable<Product> FindAsync(ProductFilter filter, CancellationToken ct = default);

    /// <summary>
    /// Registra un nuevo producto en el sistema.
    /// </summary>
    /// <param name="product">Objeto de dominio a persistir.</param>
    /// <param name="transaction">Optional transaction context.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>El ID generado para el nuevo producto.</returns>
    Task<int> AddAsync(Product product, IDbTransaction? transaction = null, CancellationToken ct = default);

    /// <summary>
    /// Actualiza la información de un producto existente.
    /// </summary>
    /// <param name="product">Objeto de dominio con los cambios.</param>
    /// <param name="transaction">Optional transaction context.</param>
    /// <param name="ct">Token de cancelación.</param>
    Task UpdateAsync(Product product, IDbTransaction? transaction = null, CancellationToken ct = default);

    /// <summary>
    /// Realiza una actualización atómica del stock de un producto.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <param name="newStock">Nueva cantidad disponible.</param>
    /// <param name="transaction">Optional transaction context.</param>
    /// <param name="ct">Token de cancelación.</param>
    Task UpdateStockAsync(int id, int newStock, IDbTransaction? transaction = null, CancellationToken ct = default);

    /// <summary>
    /// Elimina un producto del sistema.
    /// </summary>
    Task DeleteAsync(int id, IDbTransaction? transaction = null, CancellationToken ct = default);
}
