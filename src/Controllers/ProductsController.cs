using Microsoft.AspNetCore.Mvc;
using UtmMarket.Core.UseCases;
using UtmMarket.WebAPI.DTOs;
using UtmMarket.Core.Entities;

namespace UtmMarket.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(
    IGetAllProductsUseCase getAllProductsUseCase,
    IGetProductByIdUseCase getProductByIdUseCase,
    ILowStockAlertUseCase lowStockAlertUseCase,
    ICreateProductUseCase createProductUseCase,
    IDeleteProductUseCase deleteProductUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
    {
        var products = new List<ProductDto>();
        await foreach (var p in getAllProductsUseCase.ExecuteAsync())
        {
            products.Add(new ProductDto(p.ProductID, p.Name, p.SKU, p.Brand, p.Price, p.Stock));
        }
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var p = await getProductByIdUseCase.ExecuteAsync(id);
        if (p == null) return NotFound();
        return Ok(new ProductDto(p.ProductID, p.Name, p.SKU, p.Brand, p.Price, p.Stock));
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStock([FromQuery] int threshold = 10)
    {
        var products = new List<ProductDto>();
        await foreach (var p in lowStockAlertUseCase.ExecuteAsync(threshold))
        {
            products.Add(new ProductDto(p.ProductID, p.Name, p.SKU, p.Brand, p.Price, p.Stock));
        }
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] Product product)
    {
        var id = await createProductUseCase.ExecuteAsync(product);
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await deleteProductUseCase.ExecuteAsync(id);
        return NoContent();
    }
}
