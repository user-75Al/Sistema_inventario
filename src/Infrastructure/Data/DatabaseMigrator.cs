using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Infrastructure.Data;

namespace UtmMarket.Infrastructure.Data;

/// <summary>
/// Servicio para asegurar que la estructura de la base de datos esté actualizada.
/// Resuelve el error de columna 'ClienteID' faltante de forma automática.
/// </summary>
public class DatabaseMigrator(IDbConnectionFactory connectionFactory)
{
    public async Task EnsureStructureAsync()
    {
        using var connection = (SqlConnection)await connectionFactory.CreateConnectionAsync();
        
        const string createTablesSql = @"
            -- 1. Tabla Producto
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'Producto')
            BEGIN
                CREATE TABLE [dbo].[Producto] (
                    [ProductoID] INT IDENTITY(1,1) PRIMARY KEY,
                    [Nombre] NVARCHAR(100) NOT NULL,
                    [SKU] VARCHAR(20) UNIQUE NOT NULL,
                    [Marca] NVARCHAR(50) NULL,
                    [Precio] DECIMAL(19,4) NOT NULL CHECK ([Precio] >= 0),
                    [Stock] INT NOT NULL CHECK ([Stock] >= 0)
                );
                -- Seed inicial si está vacía
                INSERT INTO [dbo].[Producto] (Nombre, SKU, Marca, Precio, Stock)
                VALUES ('Laptop Pro', 'LPT-001', 'TechBrand', 25000, 10),
                       ('Mouse Inalámbrico', 'MOU-002', 'Logi', 500, 50);
            END

            -- 2. Tabla Cliente
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'Cliente')
            BEGIN
                CREATE TABLE [dbo].[Cliente] (
                    [ClienteID] INT IDENTITY(1,1) PRIMARY KEY,
                    [NombreCompleto] NVARCHAR(100) NOT NULL,
                    [Email] NVARCHAR(150) UNIQUE NOT NULL,
                    [Activo] BIT NOT NULL DEFAULT 1
                );
            END

            -- 3. Tabla Venta
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'Venta')
            BEGIN
                CREATE TABLE [dbo].[Venta] (
                    [VentaID] INT IDENTITY(1,1) PRIMARY KEY,
                    [Folio] VARCHAR(20) UNIQUE NOT NULL,
                    [FechaVenta] DATETIME NOT NULL DEFAULT GETDATE(),
                    [TotalArticulos] INT NOT NULL DEFAULT 0,
                    [TotalVenta] DECIMAL(19,4) NOT NULL DEFAULT 0,
                    [Estatus] TINYINT NOT NULL DEFAULT 1,
                    [ClienteID] INT NULL FOREIGN KEY REFERENCES [Cliente]([ClienteID])
                );
            END

            -- 4. Tabla DetalleVenta
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'DetalleVenta')
            BEGIN
                CREATE TABLE [dbo].[DetalleVenta] (
                    [DetalleID] INT IDENTITY(1,1) PRIMARY KEY,
                    [VentaID] INT NOT NULL FOREIGN KEY REFERENCES [Venta]([VentaID]) ON DELETE CASCADE,
                    [ProductoID] INT NOT NULL FOREIGN KEY REFERENCES [Producto]([ProductoID]),
                    [PrecioUnitario] DECIMAL(19,4) NOT NULL,
                    [Cantidad] INT NOT NULL,
                    [TotalDetalle] AS ([PrecioUnitario] * [Cantidad])
                );
            END";

        using var command = new SqlCommand(createTablesSql, connection);
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("[MIGRATOR] Estructura de base de datos verificada y actualizada.");
    }
}
