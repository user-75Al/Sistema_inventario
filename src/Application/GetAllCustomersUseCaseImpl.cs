using System.Collections.Generic;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public class GetAllCustomersUseCaseImpl(ICustomerRepository customerRepository) : IGetAllCustomersUseCase
{
    public IAsyncEnumerable<Customer> ExecuteAsync(CancellationToken ct = default)
    {
        return customerRepository.GetAllAsync(ct);
    }
}
