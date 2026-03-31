<role>
Actúa como un Lead Developer especializado en UX de consola profesional y orquestación de casos de uso en aplicaciones .NET 10 con Clean Architecture. Diseñas interfaces CLI resilientes y desacopladas.
</role>

<context>
Proyecto: "UtmMarket".
Tipo: Aplicación de consola.
Arquitectura: Clean Architecture.

**Estructura del proyecto:**
- **Core**: `src/Core/Entities/`, `src/Core/Repositories/`, `src/Core/UseCases/`
- **Application**: `src/Application/` (archivos sueltos)
- **Infrastructure**: `src/Infrastructure/Data/`, `src/Infrastructure/Mappers/`, `src/Infrastructure/Models/`, `src/Infrastructure/Repositories/`
</context>

<task>
Integrar la funcionalidad de consulta de ventas por rango de fechas en la interfaz de usuario de consola.
</task>

<technical_requirements>

### Verificación de Archivos Base
- **VERIFICAR** si existe `@src/Core/Entities/Sale.cs`. Si no existe, CREARLO con propiedades:
  - `SaleId` (int)
  - `Folio` (string)
  - `SaleDate` (DateTime)
  - `TotalAmount` (decimal)
- **VERIFICAR** si existe `@src/Core/UseCases/IFetchSalesByFilterUseCase.cs`. Si no existe, CREARLO con método:
  - `Task<IEnumerable<Sale>> ExecuteAsync(SaleFilter filter)`
- **VERIFICAR** si existe `SaleFilter` (puede ser en `src/Application/`). Si no existe, CREARLO con:
  - `StartDate` (DateTime)
  - `EndDate` (DateTime)

### Integración en Menú
- Agregar opción en `Program.cs`: "Consultar ventas por fecha".

### Captura de Datos
- Solicitar:
  - Fecha de Inicio
  - Fecha de Fin
- Validar con `DateTime.TryParse`.
- Reintentar en caso de error.

### Orquestación
- Crear objeto `SaleFilter`.
- Invocar `IFetchSalesByFilterUseCase`.

### Visualización
- Mostrar tabla formateada en consola:
  - Folio
  - Fecha
  - Monto Total

### Arquitectura
- Resolver servicios mediante `IServiceScope` manual en `Program.cs`.
</technical_requirements>

<execution_workflow>
1. **VERIFICAR** existencia de directorios `src/Core/Entities/`, `src/Core/UseCases/`, `src/Application/`. Crearlos si no existen.
2. **VERIFICAR** si existe `@src/Core/Entities/Sale.cs`. Si no existe, CREARLO con las propiedades especificadas.
3. **VERIFICAR** si existe `@src/Core/UseCases/IFetchSalesByFilterUseCase.cs`. Si no existe, CREARLO con la definición.
4. **VERIFICAR** si existe `SaleFilter` (en `src/Application/`). Si no existe, CREARLO.
5. **VERIFICAR** si existe la opción en el menú de `@Program.cs`. Si no, MODIFICARLO para añadir la nueva opción.
6. **VALIDAR** resolución de servicios con Scope.
</execution_workflow>

<output_format>
- Código de `@src/Core/Entities/Sale.cs` (creado si no existía)
- Código de `@src/Core/UseCases/IFetchSalesByFilterUseCase.cs` (creado si no existía)
- Código de `@src/Application/SaleFilter.cs` (creado si no existía)
- Flujo lógico de integración en `@Program.cs`
- Estructura de captura de fechas
- Secuencia de orquestación
- Diseño de tabla de salida
- Justificación arquitectónica del uso de `IServiceScope`
</output_format>

<quality_gate>
- ¿Se verificó y creó `Sale.cs` si no existía?
- ¿Se verificó y creó `IFetchSalesByFilterUseCase.cs` si no existía?
- ¿Se verificó y creó `SaleFilter.cs` si no existía?
- ¿Las fechas se validan correctamente?
- ¿Se usa `IServiceScope` manual?
- ¿La UI no accede directamente a repositorios?
- ¿La salida está formateada como tabla?
</quality_gate>