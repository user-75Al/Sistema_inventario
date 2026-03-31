namespace UtmMarket.WebAPI.DTOs;

public record ProductDto(int ProductID, string Name, string SKU, string Brand, decimal Price, int Stock);

public record CustomerDto(int CustomerID, string FullName, string Email, bool IsActive);

public record SaleItemRequestDto(int ProductId, int Quantity);

public record SaleRequestDto(int CustomerId, List<SaleItemRequestDto> Items);

public record SingleSaleRequestDto(int ProductId, int Quantity, int? CustomerId);

public record SaleResponseDto(int SaleId, string Folio, decimal Total, bool Success, string Message);

public record CreateCustomerDto(string FullName, string Email);
