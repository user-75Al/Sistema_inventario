using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace UtmMarket.Infrastructure.Data;

/// <summary>
/// Factoría de conexiones SQL Server optimizada para .NET 10 y C# 14.
/// </summary>
public class SqlConnectionFactory(IConfiguration config) : IDbConnectionFactory
{
    /// <summary>
    /// Cadena de conexión con validación inmediata mediante el keyword 'field' de C# 14.
    /// </summary>
    private string ConnectionString 
    { 
        get => field ??= config.GetConnectionString("DefaultConnection") 
                         ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada."); 
    }

    public async ValueTask<IDbConnection> CreateConnectionAsync(CancellationToken ct = default)
    {
        try 
        {
            // 1. Asegurar que la base de datos existe
            await EnsureDatabaseExistsAsync(ct);

            // 2. Conectar a la base de datos real
            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync(ct);
            return connection;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[DB ERROR] Error crítico de conexión: {ex.Message}");
            Console.ResetColor();
            throw;
        }
    }

    private async Task EnsureDatabaseExistsAsync(CancellationToken ct)
    {
        try 
        {
            var masterConnectionStr = ConnectionString.Replace("Database=basedetados", "Database=master");
            using var masterConnection = new SqlConnection(masterConnectionStr);
            await masterConnection.OpenAsync(ct);

            const string createDbSql = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'basedetados')
                BEGIN
                    CREATE DATABASE [basedetados];
                    PRINT 'Base de datos [basedetados] creada exitosamente.';
                END";
            
            using var cmd = new SqlCommand(createDbSql, masterConnection);
            await cmd.ExecuteNonQueryAsync(ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB WARNING] No se pudo verificar/crear la BD en master: {ex.Message}");
            // Intentamos continuar, tal vez la BD ya existe y solo master falló
        }
    }
}
