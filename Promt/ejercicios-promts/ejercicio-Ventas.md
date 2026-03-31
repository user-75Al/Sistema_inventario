<role>
Actúa como un Arquitecto de Software experto en Clean Architecture y desarrollo con .NET 10 y C# 14. Tu especialidad es diseñar casos de uso de negocio que interactúan con la base de datos de manera eficiente y transaccional, respetando los principios de Native AOT y evitando reflexión.
</role>

<context>
Proyecto: "UtmMarket" – aplicación de consola en .NET 10 con Clean Architecture.
Stack Técnico: .NET 10, C# 14, Microsoft.Data.SqlClient.
Base de datos: SQL Server 2022 Express, con las tablas Producto, Venta y Detalle de Venta ya creadas (ver script @02_seed_data_utm_market.sql).
Estado actual: 
- Existe la tabla `Producto` con datos semilla.
- Se requiere implementar la funcionalidad de realizar una venta, que debe descontar stock del producto y registrar la operación en las tablas `Venta` y `DetalleVenta`.
</context>

<task>
Implementar el caso de uso "Realizar Venta" que permita seleccionar un producto, especificar una cantidad, y registrar la venta, actualizando el stock correspondiente en la base de datos de forma atómica y transaccional.
</task>

<technical_requirements>

### Verificación y creación de archivos base
Antes de implementar cualquier referencia, **debes verificar la existencia de los siguientes elementos en el proyecto UtmMarket**. Si no existen, créalos siguiendo las convenciones de Clean Architecture y C# 14.

#### 1. Entidades de Dominio (src/Core/Entities/)
- **Product.cs**: Debe existir con propiedades `ProductId`, `Name`, `Price`, `Stock`. Si no, créalo.
- **Sale.cs**: Debe existir con propiedades `SaleId`, `Folio`, `SaleDate`, `TotalItems`, `TotalAmount`, `Status`. Si no, créalo.
- **SaleDetail.cs**: Debe existir con propiedades `DetailId`, `SaleId`, `ProductId`, `UnitPrice`, `Quantity`, `TotalDetail`. Si no, créalo.

#### 2. Repositorios (Interfaces en Core/Repositories/ e implementaciones en Infrastructure/Repositories/)
- **IProductRepository.cs**: Debe contener métodos `GetByIdAsync` y `UpdateAsync`. Si no, créalo.
- **ProductRepositoryImpl.cs**: Implementación con mapeo manual (SqlDataReader). Si no, créalo.
- **ISaleRepository.cs**: Debe contener métodos `AddAsync` (para cabecera) y `AddDetailAsync`. Si no, créalo.
- **SaleRepositoryImpl.cs**: Implementación con mapeo manual. Si no, créalo.

#### 3. DTOs (opcional, pueden ir en Application o Core)
- **OrderRequest.cs**: con `ProductId` y `Quantity`.
- **SaleResult.cs**: con `SaleId`, `Folio`, `Total`, `Success`, `Message`.

### Abstracción (Capa Core)
- Crear la interfaz `IPlaceOrderUseCase.cs` en `src/Core/UseCases/` con el método `Task<SaleResult> ExecuteAsync(OrderRequest request)`.

### Lógica de Aplicación (Capa Application)
- Crear la implementación `PlaceOrderUseCaseImpl.cs` en `src/Application/`.
- Inyectar mediante **Primary Constructor** los repositorios necesarios (`IProductRepository`, `ISaleRepository`).
- La implementación debe:
  1. Obtener el producto por ID.
  2. Validar existencia y stock suficiente.
  3. Generar un folio único (ej. `VENTA-{DateTime.Ticks}`).
  4. Crear registro de venta y detalle, calculando totales.
  5. Actualizar stock del producto.
  6. Ejecutar todo en una **transacción explícita** (`SqlTransaction`).
  7. Retornar `SaleResult` con el resultado.

### Registro en DI
- Registrar los repositorios y el caso de uso en `Program.cs` (ciclo `Scoped`).

</technical_requirements>

<execution_workflow>
1. **VERIFICAR** la existencia de cada archivo mencionado. Para ello, puedes asumir que el entorno de desarrollo te permite leer la estructura de carpetas (simulado).
2. **SI ALGÚN ARCHIVO NO EXISTE**, generarlo con el código apropiado (entidades POCO, interfaces, implementaciones con mapeo manual).
3. **LUEGO**, proceder a implementar el caso de uso, referenciando los elementos ya creados.
4. **REGISTRAR** servicios en DI.
5. **VALIDAR** la lógica transaccional y el descuento de stock.
</execution_workflow>

<output_format>
- Código de **todos los archivos que se hayan creado** (entidades, repositorios, DTOs, interfaces, caso de uso).
- Fragmento de registro en `Program.cs`.
- Explicación de la estrategia transaccional y de cómo se garantiza la atomicidad.
</output_format>

<quality_gate>
- ¿Se verificó la existencia de cada archivo antes de referenciarlo?
- ¿Los archivos faltantes se crearon correctamente?
- ¿Se usan **Primary Constructors** en todas las inyecciones?
- ¿El caso de uso maneja transacciones explícitas?
- ¿Se valida stock suficiente?
- ¿Los repositorios usan **mapeo manual** (sin reflexión)?
- ¿El código es compatible con Native AOT?
</quality_gate>