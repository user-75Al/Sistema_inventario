using Microsoft.Extensions.DependencyInjection;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Products
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCaseImpl>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCaseImpl>();
        services.AddScoped<ISearchProductsUseCase, SearchProductsUseCaseImpl>();
        services.AddScoped<ICreateProductUseCase, CreateProductUseCaseImpl>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCaseImpl>();
        services.AddScoped<IUpdateProductStockUseCase, UpdateProductStockUseCaseImpl>();
        services.AddScoped<ILowStockAlertUseCase, LowStockAlertUseCaseImpl>();

        // Sales
        services.AddScoped<IFetchAllSalesUseCase, FetchAllSalesUseCaseImpl>();
        services.AddScoped<IFetchSaleByIdUseCase, FetchSaleByIdUseCaseImpl>();
        services.AddScoped<IFetchSalesByFilterUseCase, FetchSalesByFilterUseCaseImpl>();
        services.AddScoped<ICreateSaleUseCase, CreateSaleUseCaseImpl>();
        services.AddScoped<IUpdateSaleStatusUseCase, UpdateSaleStatusUseCaseImpl>();
        services.AddScoped<IPlaceOrderUseCase, PlaceOrderUseCaseImpl>();
        services.AddScoped<IPlaceOrderWithCustomerUseCase, PlaceOrderWithCustomerUseCaseImpl>();

        // Customers
        services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCaseImpl>();
        services.AddScoped<IGetAllCustomersUseCase, GetAllCustomersUseCaseImpl>();
        services.AddScoped<IDeleteCustomerUseCase, DeleteCustomerUseCaseImpl>();

        // Shared / Multi-domain
        services.AddScoped<IDeleteProductUseCase, DeleteProductUseCaseImpl>();

        return services;
    }
}
