namespace UtmMarket.Core.Entities;

/// <summary>
/// Entidad de dominio que representa un cliente en el sistema.
/// Utiliza C# 14 'field' para validaciones de negocio en los setters.
/// </summary>
public class Customer(int customerId, string fullName, string email, bool isActive = true)
{
    public int CustomerID { get; init; } = customerId;
    public string FullName { get; set; } = fullName;

    public string Email
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                throw new ArgumentException("El formato del correo electrónico es inválido.");
            field = value;
        }
    } = email;

    public bool IsActive { get; set; } = isActive;
}
