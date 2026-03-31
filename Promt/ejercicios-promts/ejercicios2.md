<role>
Actúa como un Application Architect experto en casos de uso asíncronos de alto rendimiento con .NET 10 y Clean Architecture. Tu especialidad es el diseño de flujos de datos eficientes con IAsyncEnumerable.
</role>

<context>
Proyecto: "UtmMarket".
Arquitectura: Clean Architecture.
Stack: .NET 10, C# 14.

**Estructura del proyecto:**
- **Core**: `src/Core/Entities/`, `src/Core/Repositories/`, `src/Core/UseCases/`
- **Application**: `src/Application/` (archivos sueltos)
- **Infrastructure**: `src/Infrastructure/Data/`, `src/Infrastructure/Mappers/`, `src/Infrastructure/Models/`, `src/Infrastructure/Repositories/`
</context>

<task>
Diseñar e implementar el sistema de detección de inventario crítico basado en umbral configurable.
</task>

<technical_requirements>

### Verificación de Archivos Base
- **VERIFICAR** si existe `@src/Core/Entities/Product.cs`. Si no existe, CREARLO con propiedades básicas (ProductId, Name, Stock, Price).
- **VERIFICAR** si existe `@src/Core/Repositories/IProductRepository.cs`. Si no existe, CREARLO con método `GetAllAsync()` que retorne `IAsyncEnumerable<Product>`.

### Abstracción
- Crear `ILowStockAlertUseCase.cs` en `src/Core/UseCases/`.

### Implementación
- Crear `LowStockAlertUseCaseImpl.cs` en `src/Application/` (directo, sin subcarpetas).
- Inyectar `IProductRepository` mediante Primary Constructor.

### Lógica
- Recibir parámetro `threshold` entero.
- Retornar `IAsyncEnumerable<Product>`.
- Filtrar productos donde `Stock ≤ threshold`.

### Optimización
- Streaming de datos con `yield return`.
- Sin materializar colecciones completas en memoria.

### Registro DI
- Registrar el caso de uso en `Program.cs`.
</technical_requirements>

<execution_workflow>
1. **VERIFICAR** existencia de directorios `src/Core/Entities/`, `src/Core/Repositories/`, `src/Core/UseCases/`, `src/Application/`. Crearlos si no existen.
2. **VERIFICAR** si existe `@src/Core/Entities/Product.cs`. Si no existe, CREARLO con:
   - `ProductId` (int)
   - `Name` (string)
   - `Stock` (int)
   - `Price` (decimal)
3. **VERIFICAR** si existe `@src/Core/Repositories/IProductRepository.cs`. Si no existe, CREARLO con método:
   - `IAsyncEnumerable<Product> GetAllAsync()`
4. **VERIFICAR** si existe `@src/Core/UseCases/ILowStockAlertUseCase.cs`. Si no existe, CREARLO con la definición.
5. **VERIFICAR** si existe `@src/Application/LowStockAlertUseCaseImpl.cs`. Si no existe, CREARLO con la implementación.
6. **VERIFICAR** si el caso de uso está registrado en DI. Si no, añadirlo en `Program.cs`.
7. **VALIDAR** flujo streaming sin buffering.
</execution_workflow>

<output_format>
- Código de `@src/Core/Entities/Product.cs` (creado si no existía)
- Código de `@src/Core/Repositories/IProductRepository.cs` (creado si no existía)
- Código de `@src/Core/UseCases/ILowStockAlertUseCase.cs`
- Código de `@src/Application/LowStockAlertUseCaseImpl.cs`
- Registro DI en `@Program.cs`
- Explicación de eficiencia en memoria
</output_format>

<quality_gate>
- ¿Se verificó y creó `Product.cs` si no existía?
- ¿Se verificó y creó `IProductRepository.cs` si no existía?
- ¿Se utiliza `IAsyncEnumerable` correctamente?
- ¿No se cargan todos los productos en memoria?
- ¿El filtrado ocurre durante la enumeración?
- ¿El servicio está registrado en DI?
</quality_gate>
