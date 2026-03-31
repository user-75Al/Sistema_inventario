/*
-------------------------------------------------------------------------------
-- Script: 04_alter_venta_add_cliente.sql
-- Architect: Senior DB Architect / Gemini CLI
-- Description: Agrega la columna ClienteID a la tabla Venta para relacionar ventas con clientes.
-------------------------------------------------------------------------------
*/

USE [basedetados];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

-- 1. Agregar la columna ClienteID como nullable para compatibilidad.
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Venta]') AND name = 'ClienteID')
BEGIN
    ALTER TABLE [dbo].[Venta] 
    ADD [ClienteID] INT NULL;
    
    PRINT 'OK: Columna [ClienteID] agregada a la tabla [Venta].';
END
GO

-- 2. Agregar la llave foránea para garantizar integridad referencial.
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Venta_Cliente]') AND parent_object_id = OBJECT_ID(N'[dbo].[Venta]'))
BEGIN
    ALTER TABLE [dbo].[Venta]
    ADD CONSTRAINT [FK_Venta_Cliente] FOREIGN KEY ([ClienteID]) 
    REFERENCES [dbo].[Cliente] ([ClienteID]);
    
    PRINT 'OK: Llave foránea [FK_Venta_Cliente] creada.';
END
GO
