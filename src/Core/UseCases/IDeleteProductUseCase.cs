namespace UtmMarket.Core.UseCases;

/// <summary>
/// Caso de uso para eliminar un producto del catálogo.
/// </summary>
public interface IDeleteProductUseCase
{
    /// <summary>
    /// Ejecuta la eliminación del producto por su ID.
    /// </summary>
    Task ExecuteAsync(int id, CancellationToken ct = default);
}
