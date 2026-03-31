using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Data;
using UtmMarket.Infrastructure.Mappers;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Repositories;

/// <summary>
/// Implementación concreta del repositorio de productos optimizada para Native AOT.
/// Evita la reflexión mediante el uso de mapeo manual con SqlDataReader.
/// </summary>
public class ProductRepositoryImpl(IDbConnectionFactory connectionFactory) : IProductRepository
{
    private const string SelectAllSql = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto";
    private const string SelectByIdSql = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto WHERE ProductoID = @id";
    private const string InsertSql = "INSERT INTO Producto (Nombre, SKU, Marca, Precio, Stock) OUTPUT INSERTED.ProductoID VALUES (@nombre, @sku, @marca, @precio, @stock)";
    private const string UpdateSql = "UPDATE Producto SET Nombre = @nombre, SKU = @sku, Marca = @marca, Precio = @precio, Stock = @stock WHERE ProductoID = @id";
    private const string UpdateStockSql = "UPDATE Producto SET Stock = @stock WHERE ProductoID = @id";
    private const string DeleteSql = "DELETE FROM Producto WHERE ProductoID = @id";

    public async IAsyncEnumerable<Product> GetAllAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        using var command = new SqlCommand(SelectAllSql, connection);
        using var reader = await command.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            yield return MapToEntity(reader).ToDomain();
        }
    }

    public async Task<Product?> GetByIdAsync(int id, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        var connection = transaction?.Connection as SqlConnection ?? (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var ownsConnection = transaction == null;

        try
        {
            using var command = new SqlCommand(SelectByIdSql, connection, transaction as SqlTransaction);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                return MapToEntity(reader).ToDomain();
            }

            return null;
        }
        finally
        {
            if (ownsConnection) await connection.DisposeAsync();
        }
    }

    public async IAsyncEnumerable<Product> FindAsync(ProductFilter filter, [EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        
        var query = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto WHERE 1=1";
        var parameters = new List<SqlParameter>();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query += " AND Nombre LIKE @name";
            parameters.Add(new SqlParameter("@name", $"%{filter.Name}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.Brand))
        {
            query += " AND Marca LIKE @brand";
            parameters.Add(new SqlParameter("@brand", $"%{filter.Brand}%"));
        }

        if (filter.MinPrice.HasValue)
        {
            query += " AND Precio >= @minPrice";
            parameters.Add(new SqlParameter("@minPrice", filter.MinPrice.Value));
        }

        if (filter.MaxPrice.HasValue)
        {
            query += " AND Precio <= @maxPrice";
            parameters.Add(new SqlParameter("@maxPrice", filter.MaxPrice.Value));
        }

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddRange([.. parameters]);

        using var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            yield return MapToEntity(reader).ToDomain();
        }
    }

    public async Task<int> AddAsync(Product product, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        var entity = product.ToEntity();
        var connection = transaction?.Connection as SqlConnection ?? (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var ownsConnection = transaction == null;

        try
        {
            using var command = new SqlCommand(InsertSql, connection, transaction as SqlTransaction);
            
            command.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@sku", entity.SKU);
            command.Parameters.AddWithValue("@marca", entity.Marca ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@precio", entity.Precio);
            command.Parameters.AddWithValue("@stock", entity.Stock);

            return (int)await command.ExecuteScalarAsync(ct);
        }
        finally
        {
            if (ownsConnection) await connection.DisposeAsync();
        }
    }

    public async Task UpdateAsync(Product product, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        var entity = product.ToEntity();
        var connection = transaction?.Connection as SqlConnection ?? (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var ownsConnection = transaction == null;

        try
        {
            using var command = new SqlCommand(UpdateSql, connection, transaction as SqlTransaction);

            command.Parameters.AddWithValue("@id", entity.ProductoID);
            command.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@sku", entity.SKU);
            command.Parameters.AddWithValue("@marca", entity.Marca ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@precio", entity.Precio);
            command.Parameters.AddWithValue("@stock", entity.Stock);

            await command.ExecuteNonQueryAsync(ct);
        }
        finally
        {
            if (ownsConnection) await connection.DisposeAsync();
        }
    }

    public async Task UpdateStockAsync(int id, int newStock, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        var connection = transaction?.Connection as SqlConnection ?? (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var ownsConnection = transaction == null;

        try
        {
            using var command = new SqlCommand(UpdateStockSql, connection, transaction as SqlTransaction);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@stock", newStock);

            await command.ExecuteNonQueryAsync(ct);
        }
        finally
        {
            if (ownsConnection) await connection.DisposeAsync();
        }
    }

    public async Task DeleteAsync(int id, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        var connection = transaction?.Connection as SqlConnection ?? (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var ownsConnection = transaction == null;

        try
        {
            using var command = new SqlCommand(DeleteSql, connection, transaction as SqlTransaction);

            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync(ct);
        }
        finally
        {
            if (ownsConnection) await connection.DisposeAsync();
        }
    }

    /// <summary>
    /// Mapeo manual de SqlDataReader a ProductoEntity para evitar reflexión.
    /// Clave para el soporte de Native AOT.
    /// </summary>
    private static ProductoEntity MapToEntity(SqlDataReader reader)
    {
        var entity = new ProductoEntity(
            reader.GetInt32(reader.GetOrdinal("ProductoID")),
            reader.GetString(reader.GetOrdinal("SKU"))
        )
        {
            Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
            Marca = reader.IsDBNull(reader.GetOrdinal("Marca")) ? null : reader.GetString(reader.GetOrdinal("Marca")),
            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
        };

        return entity;
    }
}
