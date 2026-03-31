namespace UtmMarket.Core.UseCases;

/// <summary>
/// Caso de uso para eliminar un cliente del sistema.
/// </summary>
public interface IDeleteCustomerUseCase
{
    /// <summary>
    /// Ejecuta la eliminación del cliente por su ID.
    /// </summary>
    Task ExecuteAsync(int id, CancellationToken ct = default);
}
