analiza el siguiente pront y dime si cumple con el llenado de tablas del anterior pront que me generaste
<role>
Actúa como un Arquitecto Senior de Bases de Datos y experto en Ingeniería de Prompts. Tu objetivo es generar un script de datos semilla (seeding) para SQL Server 2022 Express con precisión técnica absoluta y datos de clientes realistas para 2025.
</role>

<context>
- Motor: Microsoft SQL Server 2022 Express.
- Destino del archivo: "/db/scripts/03_seed_data_clientes.sql".
- Tabla Objetivo: [dbo].[Cliente]
- Relación con scripts anteriores: Este script se basa en la tabla creada en `02_create_table_cliente.sql` y debe mantener coherencia con las columnas definidas.
- Esquema de Referencia (basado en ejercicio 1 y tabla cliente):
  ```sql
  CREATE TABLE [dbo].[Cliente] (
      [ClienteID] INT IDENTITY(1,1) NOT NULL,
      [NombreCompleto] NVARCHAR(100) NOT NULL,
      [Email] NVARCHAR(100) NOT NULL,
      [Telefono] NVARCHAR(20) NULL,  -- Añadido para cumplir con requerimiento del ejercicio
      [Activo] BIT NOT NULL DEFAULT 1,
      
      CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED ([ClienteID] ASC),
      CONSTRAINT [UQ_Cliente_Email] UNIQUE ([Email]),
      CONSTRAINT [CK_Cliente_Email] CHECK ([Email] LIKE '%_@__%.__%')
  );

