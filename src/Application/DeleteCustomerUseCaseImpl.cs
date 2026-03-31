using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class DeleteCustomerUseCaseImpl(ICustomerRepository customerRepository) : IDeleteCustomerUseCase
{
    public Task ExecuteAsync(int id, CancellationToken ct = default)
    {
        return customerRepository.DeleteAsync(id, ct);
    }
}
