-- ===================================================
-- Script: 03_seed_data_clientes.sql
-- Descripción: Inserción de datos semilla para la tabla Cliente (Contexto 2025)
-- Base de datos: basedetados
-- Fecha: 2026-02-26
-- Architect: Senior Database Architect
-- ===================================================

USE [basedetados];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;
BEGIN TRY
    PRINT 'Iniciando carga de datos semilla para la tabla [Cliente]...';

    -- Crear una tabla temporal para los datos a insertar y facilitar la validación de duplicados
    DECLARE @NuevosClientes TABLE (
        NombreCompleto NVARCHAR(100),
        Email NVARCHAR(100),
        Telefono NVARCHAR(20),
        Activo BIT
    );

    -- Datos realistas proyectados para 2025
    INSERT INTO @NuevosClientes (NombreCompleto, Email, Telefono, Activo)
    VALUES 
    (N'Alejandro Martínez Soler', N'a.martinez.2025@empresa.mx', N'5512345678', 1),
    (N'Lucía Fernanda Gómez', N'lucia.gomez@proyectos.com', N'5523456789', 1),
    (N'Roberto Carlos Treviño', N'r.trevino@servicios.net', N'8134567890', 1),
    (N'Mariana Silva Pantoja', N'm.silva@estudio2025.org', N'3345678901', 1),
    (N'Juan Pablo Guerrero', N'jp.guerrero@tecnologia.io', N'5556789012', 1),
    (N'Beatriz Adriana Luna', N'b.luna@consultoria.com.mx', N'2226789012', 0), -- Cliente inactivo
    (N'Carlos Eduardo Ruiz', N'cruiz.2025@market.net', N'4427890123', 1),
    (N'Sofía Isabel Valdés', N's.valdes@diseno.com', N'5568901234', 1),
    (N'Ricardo Montalván', N'rmontalvan@logistica.mx', N'8189012345', 1),
    (N'Elena María Castañeda', N'e.castaneda@finanzas.com', N'3390123456', 1),
    (N'Daniel Ortega Pozos', N'dortega@redes2025.net', N'5501234567', 1),
    (N'Gabriela Ruiz Esparza', N'g.ruiz@soluciones.io', N'2212345678', 1),
    (N'Fernando Valenzuela', N'f.valenzuela@deportes.mx', N'5523456780', 1),
    (N'Patricia Luján', N'p.lujan@educacion.org', N'8134567891', 1),
    (N'Jorge Luis Borges Ramos', N'j.borges@literatura.com', N'3345678902', 1);

    -- Inserción final evitando correos duplicados en la base de datos real
    INSERT INTO [dbo].[Cliente] (NombreCompleto, Email, Telefono, Activo)
    SELECT nc.NombreCompleto, nc.Email, nc.Telefono, nc.Activo
    FROM @NuevosClientes nc
    WHERE NOT EXISTS (
        SELECT 1 FROM [dbo].[Cliente] c WHERE c.Email = nc.Email
    );

    DECLARE @FilasInsertadas INT = @@ROWCOUNT;
    PRINT 'Carga finalizada. Registros nuevos insertados: ' + CAST(@FilasInsertadas AS VARCHAR(10));

    COMMIT TRANSACTION;
    PRINT 'Transacción completada con éxito.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT 'Error detectado durante la carga: ' + ERROR_MESSAGE();
    THROW;
END CATCH
GO
