using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación del caso de uso para crear clientes.
/// </summary>
public class CreateCustomerUseCaseImpl(ICustomerRepository customerRepository) : ICreateCustomerUseCase
{
    public async Task<int> ExecuteAsync(Customer customer, CancellationToken ct = default)
    {
        // Validación de negocio: No permitir emails duplicados
        var existing = await customerRepository.GetByEmailAsync(customer.Email, ct);
        if (existing != null)
        {
            throw new InvalidOperationException($"El cliente con el correo {customer.Email} ya se encuentra registrado.");
        }

        return await customerRepository.AddAsync(customer, ct);
    }
}
