/*
-------------------------------------------------------------------------------
-- Script: 03_create_table_cliente.sql
-- Architect: Senior DB Architect / Gemini CLI
-- Description: DDL para la tabla de clientes en UtmMarket.
-------------------------------------------------------------------------------
*/

USE [basedetados];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Cliente] (
        [ClienteID]      INT IDENTITY(1,1) NOT NULL,
        [NombreCompleto] NVARCHAR(100)      NOT NULL,
        [Email]          NVARCHAR(150)      NOT NULL,
        [Activo]         BIT                NOT NULL DEFAULT 1,
        
        CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED ([ClienteID] ASC),
        CONSTRAINT [UQ_Cliente_Email] UNIQUE ([Email])
    );
    PRINT 'OK: Tabla [Cliente] creada.';
END
GO
