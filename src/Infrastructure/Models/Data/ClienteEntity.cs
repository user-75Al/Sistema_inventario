namespace UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Modelo de persistencia para la tabla [Cliente].
/// Diseñado para mapeo manual compatible con Native AOT.
/// </summary>
public class ClienteEntity(int clienteId, string fullName, string email)
{
    public int ClienteID { get; init; } = clienteId;
    public string NombreCompleto { get; set; } = fullName;
    public string Email { get; set; } = email;
    public bool Activo { get; set; } = true;
}
