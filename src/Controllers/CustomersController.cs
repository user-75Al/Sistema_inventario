using Microsoft.AspNetCore.Mvc;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.WebAPI.DTOs;

namespace UtmMarket.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(
    IGetAllCustomersUseCase getAllCustomersUseCase,
    ICustomerRepository customerRepository,
    ICreateCustomerUseCase createCustomerUseCase,
    IDeleteCustomerUseCase deleteCustomerUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
    {
        var customers = new List<CustomerDto>();
        await foreach (var c in getAllCustomersUseCase.ExecuteAsync())
        {
            customers.Add(new CustomerDto(c.CustomerID, c.FullName, c.Email, c.IsActive));
        }
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        var c = await customerRepository.GetByIdAsync(id);
        if (c == null) return NotFound();

        return Ok(new CustomerDto(c.CustomerID, c.FullName, c.Email, c.IsActive));
    }

    [HttpGet("by-email")]
    public async Task<ActionResult<CustomerDto>> GetByEmail([FromQuery] string email)
    {
        var c = await customerRepository.GetByEmailAsync(email);
        if (c == null) return NotFound();

        return Ok(new CustomerDto(c.CustomerID, c.FullName, c.Email, c.IsActive));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateCustomerDto dto)
    {
        var customer = new Customer(0, dto.FullName, dto.Email);
        var id = await createCustomerUseCase.ExecuteAsync(customer);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await deleteCustomerUseCase.ExecuteAsync(id);
        return NoContent();
    }
}
