using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
/// Implementación avanzada del repositorio de ventas para Native AOT.
/// Gestiona la jerarquía Venta-Detalle mediante mapeo manual y consultas optimizadas.
/// </summary>
public class SaleRepositoryImpl(IDbConnectionFactory connectionFactory) : ISaleRepository
{
    private const string SelectSaleByIdSql = @"
        SELECT v.VentaID, v.Folio, v.FechaVenta, v.TotalArticulos, v.TotalVenta, v.Estatus, v.ClienteID, c.NombreCompleto 
        FROM Venta v 
        LEFT JOIN Cliente c ON v.ClienteID = c.ClienteID 
        WHERE v.VentaID = @id";
    private const string SelectDetailsBySaleIdSql = @"
        SELECT dv.DetalleID, dv.VentaID, dv.ProductoID, dv.PrecioUnitario, dv.Cantidad, dv.TotalDetalle,
               p.Nombre, p.SKU, p.Marca, p.Precio, p.Stock
        FROM DetalleVenta dv
        INNER JOIN Producto p ON dv.ProductoID = p.ProductoID
        WHERE dv.VentaID = @ventaId";

    public async IAsyncEnumerable<Sale> GetAllAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        const string sql = @"
            SELECT v.VentaID, v.Folio, v.FechaVenta, v.TotalArticulos, v.TotalVenta, v.Estatus, v.ClienteID, c.NombreCompleto 
            FROM Venta v 
            LEFT JOIN Cliente c ON v.ClienteID = c.ClienteID 
            ORDER BY v.FechaVenta DESC";
        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync(ct);

        var salesList = new List<VentaEntity>();
        while (await reader.ReadAsync(ct))
        {
            salesList.Add(MapVentaEntity(reader));
        }
        reader.Close();

        foreach (var vEntity in salesList)
        {
            yield return await GetSaleWithDetailsAsync(vEntity, connection, ct, transaction: null);
        }
    }

    public async Task<Sale?> GetByIdAsync(int id, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        var connection = transaction?.Connection as SqlConnection ?? (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var ownsConnection = transaction == null;

        try
        {
            using var command = new SqlCommand(SelectSaleByIdSql, connection, transaction as SqlTransaction);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                var vEntity = MapVentaEntity(reader);
                reader.Close();
                return await GetSaleWithDetailsAsync(vEntity, connection, ct, transaction as SqlTransaction);
            }

            return null;
        }
        finally
        {
            if (ownsConnection) await connection.DisposeAsync();
        }
    }

    public async IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, [EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var query = @"
            SELECT v.VentaID, v.Folio, v.FechaVenta, v.TotalArticulos, v.TotalVenta, v.Estatus, v.ClienteID, c.NombreCompleto 
            FROM Venta v 
            LEFT JOIN Cliente c ON v.ClienteID = c.ClienteID 
            WHERE 1=1";
        var parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(filter.Folio))
        {
            query += " AND Folio = @folio";
            parameters.Add(new SqlParameter("@folio", filter.Folio));
        }

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddRange([.. parameters]);

        using var reader = await command.ExecuteReaderAsync(ct);
        var salesToProcess = new List<VentaEntity>();
        while (await reader.ReadAsync(ct))
        {
            salesToProcess.Add(MapVentaEntity(reader));
        }
        reader.Close();

        foreach (var entity in salesToProcess)
        {
            yield return await GetSaleWithDetailsAsync(entity, connection, ct, transaction: null);
        }
    }

    public async Task<Sale> AddAsync(Sale sale, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        var (header, details) = sale.ToEntity();
        
        // Si hay una transacción, usamos su conexión. Si no, creamos una nueva.
        var connection = transaction?.Connection as SqlConnection ?? (SqlConnection)await connectionFactory.CreateConnectionAsync(ct);
        var ownsConnection = transaction == null;
        var sqlTransaction = transaction as SqlTransaction;

        // Si no se inyectó transacción, creamos una local.
        bool isLocalTransaction = sqlTransaction == null;
        if (isLocalTransaction)
        {
            sqlTransaction = connection.BeginTransaction();
        }

        try
        {
            const string insertVenta = "INSERT INTO Venta (Folio, FechaVenta, TotalArticulos, TotalVenta, Estatus, ClienteID) OUTPUT INSERTED.VentaID VALUES (@folio, @fecha, @articulos, @total, @estatus, @cId)";
            using var vCmd = new SqlCommand(insertVenta, connection, sqlTransaction);
            vCmd.Parameters.AddWithValue("@folio", header.Folio);
            vCmd.Parameters.AddWithValue("@fecha", header.FechaVenta);
            vCmd.Parameters.AddWithValue("@articulos", header.TotalArticulos);
            vCmd.Parameters.AddWithValue("@total", header.TotalVenta);
            vCmd.Parameters.AddWithValue("@estatus", header.Estatus);
            vCmd.Parameters.AddWithValue("@cId", (object?)header.ClienteID ?? DBNull.Value);

            int newId = (int)await vCmd.ExecuteScalarAsync(ct);

            foreach (var detail in details)
            {
                const string insertDetail = "INSERT INTO DetalleVenta (VentaID, ProductoID, PrecioUnitario, Cantidad) VALUES (@vId, @pId, @precio, @qty)";
                using var dCmd = new SqlCommand(insertDetail, connection, sqlTransaction);
                dCmd.Parameters.AddWithValue("@vId", newId);
                dCmd.Parameters.AddWithValue("@pId", detail.ProductoID);
                dCmd.Parameters.AddWithValue("@precio", detail.PrecioUnitario);
                dCmd.Parameters.AddWithValue("@qty", detail.Cantidad);
                await dCmd.ExecuteNonQueryAsync(ct);
            }

            // Solo hacemos commit si la transacción fue creada en este método.
            if (isLocalTransaction && sqlTransaction != null)
            {
                await sqlTransaction.CommitAsync(ct);
            }

            // En lugar de volver a consultar la base de datos (que puede causar bloqueos en la misma transacción),
            // aprovechamos que ya tenemos todos los datos y el nuevo ID.
            // Creamos un objeto de dominio Sale con el ID generado.
            var resultSale = new Sale(newId, sale.Folio)
            {
                SaleDate = sale.SaleDate,
                Status = sale.Status
            };
            foreach (var detail in sale.Details)
            {
                resultSale.Details.Add(detail);
            }

            return resultSale;
        }
        catch
        {
            // Solo hacemos rollback si la transacción fue creada en este método.
            if (isLocalTransaction && sqlTransaction != null)
            {
                await sqlTransaction.RollbackAsync(ct);
            }
            throw;
        }
        finally
        {
            // Liberamos la transacción local
            if (isLocalTransaction && sqlTransaction != null)
            {
                await sqlTransaction.DisposeAsync();
            }

            // Si creamos la conexión nosotros, la liberamos.
            if (ownsConnection)
            {
                await connection.DisposeAsync();
            }
        }
    }

    public async Task UpdateAsync(Sale sale, IDbTransaction? transaction = null, CancellationToken ct = default)
    {
        // Lógica similar a AddAsync pero con UPDATE y gestión de borrado/inserción de detalles
        throw new NotImplementedException("Update logic requires complex delta management in pure ADO.NET.");
    }

    private async Task<Sale> GetSaleWithDetailsAsync(VentaEntity vEntity, SqlConnection connection, CancellationToken ct, SqlTransaction? transaction = null)
    {
        using var dCmd = new SqlCommand(SelectDetailsBySaleIdSql, connection, transaction);
        dCmd.Parameters.AddWithValue("@ventaId", vEntity.VentaID);
        using var dReader = await dCmd.ExecuteReaderAsync(ct);

        var detailList = new List<(DetalleVentaEntity, Product)>();
        while (await dReader.ReadAsync(ct))
        {
            var dEntity = MapDetalleVentaEntity(dReader);
            var product = MapProductFromDetail(dReader);
            detailList.Add((dEntity, product));
        }
        dReader.Close();

        return vEntity.ToDomain(detailList);
    }

    private static VentaEntity MapVentaEntity(SqlDataReader reader) =>
        new VentaEntity(reader.GetInt32(0), reader.GetString(1))
        {
            FechaVenta = reader.GetDateTime(2),
            TotalArticulos = reader.GetInt32(3),
            TotalVenta = reader.GetDecimal(4),
            Estatus = reader.GetByte(5),
            ClienteID = reader.IsDBNull(6) ? null : reader.GetInt32(6),
            NombreCliente = reader.IsDBNull(7) ? "Público General" : reader.GetString(7)
        };

    private static DetalleVentaEntity MapDetalleVentaEntity(SqlDataReader reader) =>
        new DetalleVentaEntity(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2))
        {
            PrecioUnitario = reader.GetDecimal(3),
            Cantidad = reader.GetInt32(4),
            TotalDetalle = reader.GetDecimal(5)
        };

    private static Product MapProductFromDetail(SqlDataReader reader) =>
        new Product(
            reader.GetInt32(2),
            reader.GetString(6),
            reader.GetString(7),
            reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
            reader.GetDecimal(9),
            reader.GetInt32(10)
        );
}
