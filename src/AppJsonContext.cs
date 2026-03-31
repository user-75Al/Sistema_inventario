using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using UtmMarket.WebAPI.DTOs;

namespace UtmMarket.WebAPI;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ProductDto))]
[JsonSerializable(typeof(IEnumerable<ProductDto>))]
[JsonSerializable(typeof(CustomerDto))]
[JsonSerializable(typeof(IEnumerable<CustomerDto>))]
[JsonSerializable(typeof(SaleRequestDto))]
[JsonSerializable(typeof(SingleSaleRequestDto))]
[JsonSerializable(typeof(SaleResponseDto))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(CreateCustomerDto))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
