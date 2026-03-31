-- ===================================================
-- Script: 02_create_table_cliente.sql
-- Descripción: Crea la tabla Cliente para el módulo de Gestión de Clientes
-- Base de datos: basedetados
-- Fecha: 2026-02-26
-- Architect: Senior Database Architect
-- ===================================================

USE [basedetados];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- Verificar si la tabla ya existe y crearla si no
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND type in (N'U'))
BEGIN
    PRINT 'Iniciando creación de la tabla Cliente...';
    
    CREATE TABLE [dbo].[Cliente] (
        [ClienteID] INT IDENTITY(1,1) NOT NULL,
        [NombreCompleto] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Telefono] NVARCHAR(20) NULL,
        [Activo] BIT NOT NULL CONSTRAINT [DF_Cliente_Activo] DEFAULT 1,
        
        -- Primary Key: Eficiencia con INT IDENTITY
        CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED ([ClienteID] ASC),
        
        -- Unique Key para Email: Garantiza unicidad de correos
        CONSTRAINT [UQ_Cliente_Email] UNIQUE ([Email]),
        
        -- Check para formato básico de Email: Validación de integridad en origen
        CONSTRAINT [CK_Cliente_Email] CHECK ([Email] LIKE '%_@__%.__%')
    );
    
    PRINT 'Tabla Cliente creada exitosamente.';
    
    -- Comentarios de documentación (Extended Properties)
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Tabla que almacena la información de clientes del sistema', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'Cliente';
    
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Identificador único del cliente (autoincremental)', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'Cliente', 
        @level2type = N'COLUMN', @level2name = N'ClienteID';
    
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Nombre completo del cliente', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'Cliente', 
        @level2type = N'COLUMN', @level2name = N'NombreCompleto';
    
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Correo electrónico único del cliente', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'Cliente', 
        @level2type = N'COLUMN', @level2name = N'Email';
    
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Teléfono del cliente (10 dígitos)', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'Cliente', 
        @level2type = N'COLUMN', @level2name = N'Telefono';
    
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Indica si el cliente está activo (1) o inactivo (0)', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'Cliente', 
        @level2type = N'COLUMN', @level2name = N'Activo';
END
ELSE
BEGIN
    PRINT 'La tabla Cliente ya existe en [basedetados]. No se realizaron cambios.';
END
GO
