using System.Data;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Data;
using UtmMarket.Infrastructure.Mappers;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Repositories;

/// <summary>
/// Implementación asíncrona y AOT-Safe del repositorio de clientes.
/// </summary>
public class CustomerRepositoryImpl(IDbConnectionFactory connectionFactory) : ICustomerRepository
{
    private const string SelectByEmailSql = "SELECT ClienteID, NombreCompleto, Email, Activo FROM Cliente WHERE Email = @email";
    private const string SelectByIdSql = "SELECT ClienteID, NombreCompleto, Email, Activo FROM Cliente WHERE ClienteID = @id";
    private const string SelectAllSql = "SELECT ClienteID, NombreCompleto, Email, Activo FROM Cliente";
    private const string InsertSql = "INSERT INTO Cliente (NombreCompleto, Email, Activo) OUTPUT INSERTED.ClienteID VALUES (@nombre, @email, @activo)";
    private const string DeleteSql = "DELETE FROM Cliente WHERE ClienteID = @id";

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        using var command = new SqlCommand(SelectByEmailSql, connection);
        command.Parameters.AddWithValue("@email", email);

        using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapToEntity(reader).ToDomain();
        }

        return null;
    }

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        using var command = new SqlCommand(SelectByIdSql, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapToEntity(reader).ToDomain();
        }

        return null;
    }

    public async IAsyncEnumerable<Customer> GetAllAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        using var command = new SqlCommand(SelectAllSql, connection);

        using var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            yield return MapToEntity(reader).ToDomain();
        }
    }

    public async Task<int> AddAsync(Customer customer, CancellationToken ct = default)
    {
        var entity = customer.ToEntity();
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        using var command = new SqlCommand(InsertSql, connection);
        
        command.Parameters.AddWithValue("@nombre", entity.NombreCompleto);
        command.Parameters.AddWithValue("@email", entity.Email);
        command.Parameters.AddWithValue("@activo", entity.Activo);

        return (int)await command.ExecuteScalarAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        using var command = new SqlCommand(DeleteSql, connection);
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync(ct);
    }

    private static ClienteEntity MapToEntity(SqlDataReader reader)
    {
        return new ClienteEntity(
            reader.GetInt32(reader.GetOrdinal("ClienteID")),
            reader.GetString(reader.GetOrdinal("NombreCompleto")),
            reader.GetString(reader.GetOrdinal("Email"))
        )
        {
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
        };
    }
}
