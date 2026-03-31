using Microsoft.AspNetCore.Mvc;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.WebAPI.DTOs;

namespace UtmMarket.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(
    IPlaceOrderWithCustomerUseCase placeOrderUseCase,
    IFetchSalesByFilterUseCase fetchSalesUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SaleResponseDto>> Create([FromBody] SingleSaleRequestDto dto)
    {
        var items = new List<OrderItemRequest> { new OrderItemRequest(dto.ProductId, dto.Quantity) };
        var request = new OrderWithCustomerRequest(dto.CustomerId ?? 0, items);
        
        var result = await placeOrderUseCase.ExecuteAsync(request);
        
        return Ok(new SaleResponseDto(
            result.SaleId, 
            result.Folio, 
            result.Total, 
            result.Success, 
            result.Message));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleResponseDto>>> Get([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var filter = new SaleFilter { StartDate = startDate, EndDate = endDate };
        var salesList = new List<SaleResponseDto>();
        
        var sales = await fetchSalesUseCase.ExecuteAsync(filter);
        foreach (var s in sales)
        {
            salesList.Add(new SaleResponseDto(s.SaleID, s.Folio, s.TotalSale, true, ""));
        }
        
        return Ok(salesList);
    }
}
