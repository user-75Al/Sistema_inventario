/*
-------------------------------------------------------------------------------
-- Script: 01_create_structure_utm_market.sql
-- Architect: Senior DB Architect / Gemini CLI
-- Description: DDL para la estructura de la base de datos UtmMarket.
-- Database: basedetados
-- Engine: Microsoft SQL Server 2022 Express
-------------------------------------------------------------------------------
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

USE [basedetados];
GO

-- ============================================================================
-- 1. Tabla: Producto
-- Almacena el catálogo de artículos con control de stock y SKU único.
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Producto] (
        [ProductoID] INT IDENTITY(1,1) NOT NULL,
        [Nombre]     NVARCHAR(100)      NOT NULL,
        [SKU]        VARCHAR(20)        NOT NULL,
        [Marca]      NVARCHAR(50)       NULL,
        [Precio]     DECIMAL(19,4)      NOT NULL,
        [Stock]      INT                NOT NULL,
        
        CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED ([ProductoID] ASC),
        CONSTRAINT [UQ_Producto_SKU] UNIQUE ([SKU]),
        CONSTRAINT [CK_Producto_Precio] CHECK ([Precio] >= 0),
        CONSTRAINT [CK_Producto_Stock]  CHECK ([Stock] >= 0)
    );
    PRINT 'OK: Tabla [Producto] creada.';
END
GO

-- ============================================================================
-- 2. Tabla: Venta
-- Cabecera de transacciones comerciales con seguimiento de estatus.
-- Estatus: 1: Pendiente, 2: Completada, 3: Cancelada
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Venta]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Venta] (
        [VentaID]        INT IDENTITY(1,1) NOT NULL,
        [Folio]          VARCHAR(20)       NOT NULL,
        [FechaVenta]     DATETIME          NOT NULL DEFAULT GETDATE(),
        [TotalArticulos] INT               NOT NULL,
        [TotalVenta]     DECIMAL(19,4)     NOT NULL,
        [Estatus]        TINYINT           NOT NULL, 
        
        CONSTRAINT [PK_Venta] PRIMARY KEY CLUSTERED ([VentaID] ASC),
        CONSTRAINT [UQ_Venta_Folio] UNIQUE ([Folio]),
        CONSTRAINT [CK_Venta_TotalArticulos] CHECK ([TotalArticulos] >= 0),
        CONSTRAINT [CK_Venta_TotalVenta]     CHECK ([TotalVenta] >= 0),
        CONSTRAINT [CK_Venta_Estatus]        CHECK ([Estatus] IN (1, 2, 3))
    );
    PRINT 'OK: Tabla [Venta] creada.';
END
GO

-- ============================================================================
-- 3. Tabla: DetalleVenta
-- Desglose de productos por venta (Relación N:N entre Producto y Venta).
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleVenta]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DetalleVenta] (
        [DetalleID]      INT IDENTITY(1,1) NOT NULL,
        [VentaID]        INT               NOT NULL,
        [ProductoID]     INT               NOT NULL,
        [PrecioUnitario] DECIMAL(19,4)     NOT NULL,
        [Cantidad]       INT               NOT NULL,
        [TotalDetalle]   DECIMAL(19,4)     NOT NULL,
        
        CONSTRAINT [PK_DetalleVenta] PRIMARY KEY CLUSTERED ([DetalleID] ASC),
        CONSTRAINT [FK_DetalleVenta_Venta] FOREIGN KEY ([VentaID]) 
            REFERENCES [dbo].[Venta] ([VentaID]) ON DELETE CASCADE,
        CONSTRAINT [FK_DetalleVenta_Producto] FOREIGN KEY ([ProductoID]) 
            REFERENCES [dbo].[Producto] ([ProductoID]),
        CONSTRAINT [CK_DetalleVenta_PrecioUnitario] CHECK ([PrecioUnitario] >= 0),
        CONSTRAINT [CK_DetalleVenta_Cantidad]       CHECK ([Cantidad] > 0),
        CONSTRAINT [CK_DetalleVenta_TotalDetalle]   CHECK ([TotalDetalle] >= 0)
    );
    PRINT 'OK: Tabla [DetalleVenta] creada.';
END
GO
