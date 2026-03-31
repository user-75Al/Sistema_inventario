using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Mappers;

/// <summary>
/// Mapeador estático para la entidad Customer.
/// </summary>
public static class CustomerMapper
{
    public static Customer ToDomain(this ClienteEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new Customer(
            customerId: entity.ClienteID,
            fullName: entity.NombreCompleto,
            email: entity.Email,
            isActive: entity.Activo
        );
    }

    public static ClienteEntity ToEntity(this Customer domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return new ClienteEntity(domain.CustomerID, domain.FullName, domain.Email)
        {
            Activo = domain.IsActive
        };
    }
}
