<role>
Actúa como un Arquitecto de Bases de Datos Senior y experto en Ingeniería de Prompts. Tu objetivo es generar un script SQL de alta calidad para la tabla de Clientes y asegurar su persistencia en el sistema de archivos del usuario utilizando tus herramientas de edición de código.
</role>

<context>
- Motor de DB: Microsoft SQL Server 2022 Express (Considera el límite de 10GB y optimiza tipos de datos).
- Base de Datos: "basedetados" (La base de datos ya existe, asegúrate de usarla).
- Relación con ejercicios anteriores: Esta tabla corresponde a la entidad `Customer` definida en el módulo de Gestión de Clientes (Ejercicio 1).
- Ubicación de Salida: "/db/scripts/02_create_table_cliente.sql".
</context>

<task>
1. Genera un script DDL completo para la tabla: Cliente.
2. Define tipos de datos precisos: usa NVARCHAR para textos que requieran soporte Unicode, INT para identificadores, BIT para campos booleanos y NVARCHAR(20) para teléfonos.
3. Establece un Identificador Único (Primary Key) usando INT IDENTITY para eficiencia.
4. Implementa una restricción UNIQUE en el campo Email para garantizar la unicidad de los correos electrónicos.
5. Agrega una restricción CHECK para validar el formato básico del Email.
6. Define un valor por defecto para el campo Activo (1 = activo).
</task>

<requirements>
Crea la tabla "Cliente" con las siguientes columnas y restricciones, basadas en el modelo de dominio del ejercicio de Gestión de Clientes:

1. Tabla "Cliente":
   - **ClienteID**: `INT IDENTITY(1,1) NOT NULL` – Identificador único del cliente (Primary Key).
   - **NombreCompleto**: `NVARCHAR(100) NOT NULL` – Nombre completo del cliente.
   - **Email**: `NVARCHAR(100) NOT NULL` – Correo electrónico del cliente.
     - Debe ser único: `CONSTRAINT [UQ_Cliente_Email] UNIQUE ([Email])`.
     - Debe tener una restricción CHECK para formato básico: `CONSTRAINT [CK_Cliente_Email] CHECK ([Email] LIKE '%_@__%.__%')`.
   - **Telefono**: `NVARCHAR(20) NULL` – Número telefónico del cliente (10 dígitos, sin guiones).
   - **Activo**: `BIT NOT NULL DEFAULT 1` – Indica si el cliente está activo (1) o inactivo (0). Por defecto, verdadero.

Asegura que el script sea idempotente usando "IF NOT EXISTS" o verificaciones previas de objetos.

El script debe incluir:
- Verificación de existencia de la base de datos (opcional, pero recomendado).
- Verificación de existencia de la tabla antes de crearla.
- Creación de la tabla con todas sus columnas.
- Creación de la Primary Key.
- Creación de la restricción UNIQUE para Email.
- Creación de la restricción CHECK para formato de Email.
- Comentarios explicativos para cada elemento.
</requirements>

<file_system_instructions>
- Antes de escribir, verifica si el directorio "/db/scripts/" existe. Si no, créalo.
- Usa barras diagonales (/) para la ruta del archivo para evitar errores de escape en Windows.
- Guarda el contenido generado en el archivo especificado.
- Una vez guardado, confirma la operación y resume las características de seguridad e integridad implementadas en el script.
</file_system_instructions>

<sql_best_practices>
- Incluye `USE [basedetados];` al inicio del script para asegurar que se ejecute en la base de datos correcta.
- Incluye `SET NOCOUNT ON` y `SET XACT_ABORT ON` al inicio del script.
- Usa comentarios detallados que expliquen la arquitectura de la tabla y cada restricción.
- Asegura que el script sea idempotente (puedas ejecutarlo varias veces sin errores) usando `IF NOT EXISTS` para verificar la existencia de la tabla y las restricciones.
- Considera envolver la creación en un bloque `BEGIN TRANSACTION` / `COMMIT` para garantizar atomicidad (opcional pero recomendado).
</sql_best_practices>

<output_format>
El script final debe tener la siguiente estructura:

```sql
-- ===================================================
-- Script: 02_create_table_cliente.sql
-- Descripción: Crea la tabla Cliente para el módulo de Gestión de Clientes
-- Base de datos: basedetados
-- Fecha: [fecha actual]
-- ===================================================

USE [basedetados];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- Verificar si la tabla ya existe y crearla si no
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Cliente] (
        [ClienteID] INT IDENTITY(1,1) NOT NULL,
        [NombreCompleto] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Telefono] NVARCHAR(20) NULL,
        [Activo] BIT NOT NULL CONSTRAINT [DF_Cliente_Activo] DEFAULT 1,
        
        -- Primary Key
        CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED ([ClienteID] ASC),
        
        -- Unique Key para Email
        CONSTRAINT [UQ_Cliente_Email] UNIQUE ([Email]),
        
        -- Check para formato básico de Email
        CONSTRAINT [CK_Cliente_Email] CHECK ([Email] LIKE '%_@__%.__%')
    );
    
    PRINT 'Tabla Cliente creada exitosamente.';
    
    -- Comentarios de documentación
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
    PRINT 'La tabla Cliente ya existe. No se realizaron cambios.';
END
GO