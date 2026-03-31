<role>
Actúa como un Software Architect experto en Clean Architecture y desarrollo Native AOT con .NET 10. Diseñas modelos de dominio robustos, repositorios AOT-Safe y contratos de persistencia optimizados sin reflexión.
</role>

<context>
Proyecto: "UtmMarket" (Console App).
Arquitectura: Clean Architecture.
Stack Técnico: .NET 10, C# 14, Microsoft.Data.SqlClient.

**Estructura del proyecto:**
- **Core**: `src/Core/Entities/`, `src/Core/Repositories/`, `src/Core/UseCases/`
- **Application**: `src/Application/` (archivos sueltos)
- **Infrastructure**: 
  - `src/Infrastructure/Data/`
  - `src/Infrastructure/Mappers/`
  - `src/Infrastructure/Models/`
  - `src/Infrastructure/Repositories/`

Restricciones de Plataforma:
- Sin reflexión ni generación dinámica de IL.
- Mapeo manual obligatorio mediante SqlDataReader.
- Inyección de dependencias explícita.
</context>

<task>
Diseñar e implementar el módulo completo de Gestión de Clientes compatible con Native AOT.
</task>

<technical_requirements>

### Modelo de Dominio
- Crear `Customer.cs` en `src/Core/Entities/`.
- Propiedades:
  - `CustomerId`
  - `FullName`
  - `Email`
  - `IsActive`
- Validar `Email` en el setter usando la palabra clave `field` de C# 14.

### Contrato de Persistencia
- Crear `ICustomerRepository.cs` en `src/Core/Repositories/`.
- Métodos requeridos:
  - `GetByEmailAsync(string email)`
  - `AddAsync(Customer customer)`

### Implementación de Infraestructura
- Crear `CustomerRepositoryImpl.cs` en `src/Infrastructure/Repositories/`.
- Inyección mediante Primary Constructor.
- Implementación asíncrona.
- Acceso a datos usando:
  - `SqlCommand`
  - `SqlDataReader`
  - Mapeo manual sin Dapper dinámico.

### Registro de Servicios
- Registrar el repositorio en el contenedor de DI en `Program.cs`.
</technical_requirements>

<execution_workflow>
1. **VERIFICAR** existencia de directorios `src/Core/Entities/`, `src/Core/Repositories/`, `src/Infrastructure/Repositories/`. Crearlos si no existen.
2. **VERIFICAR** si ya existe `Customer.cs`. Si no existe, crearlo con las propiedades especificadas.
3. **VERIFICAR** si ya existe `ICustomerRepository.cs`. Si no existe, crearlo con los métodos requeridos.
4. **VERIFICAR** si ya existe `CustomerRepositoryImpl.cs`. Si no existe, crearlo con la implementación manual usando `SqlDataReader`.
5. **VERIFICAR** si ya está registrado en DI. Si no, añadir el registro en `Program.cs`.
6. **VALIDAR** compatibilidad Native AOT.
</execution_workflow>

<output_format>
- Código completo de `@src/Core/Entities/Customer.cs` (creado o verificado)
- Código completo de `@src/Core/Repositories/ICustomerRepository.cs` (creado o verificado)
- Código completo de `@src/Infrastructure/Repositories/CustomerRepositoryImpl.cs` (creado o verificado)
- Código de registro en DI (añadido si no existía)
- Justificación técnica de compatibilidad AOT
</output_format>

<quality_gate>
- ¿El Email se valida en el setter con `field`?
- ¿No existe reflexión en ninguna capa?
- ¿El repositorio es completamente asíncrono?
- ¿Se usa `SqlDataReader` manualmente?
- ¿La inyección usa Primary Constructor?
</quality_gate>+
/
