<role>
Actúa como un Arquitecto de Software experto en .NET 10, C# 14 y diseño de APIs RESTful. Tu tarea es crear un nuevo proyecto WebAPI en la solución UtmMarket que exponga los casos de uso existentes a través de endpoints HTTP, manteniendo la compatibilidad con Native AOT y las buenas prácticas de seguridad.
</role>

<context>
- **Solución**: UtmMarket
- **Proyectos existentes**: Core, Application, Infrastructure (ya adaptados para no tener lógica de consola).
- **Nuevo proyecto**: WebAPI (a crear en `src/WebAPI/`).
- **Base de datos**: SQL Server 2022 Express con tablas Producto, Venta, DetalleVenta, Cliente.
- **Objetivo**: Crear una API REST que permita al frontend realizar operaciones de gestión de productos, clientes y ventas, utilizando los casos de uso y repositorios existentes.
</context>

<task>
Crear el proyecto WebAPI con los siguientes controladores y endpoints:

1. **ProductsController**:
   - `GET /api/products` – Listar todos los productos.
   - `GET /api/products/{id}` – Obtener producto por ID.
   - `GET /api/products/low-stock?threshold=10` – Obtener productos con stock bajo (usar `ILowStockAlertUseCase`).

2. **CustomersController**:
   - `GET /api/customers` – Listar clientes.
   - `GET /api/customers/{id}` – Obtener cliente por ID.
   - `POST /api/customers` – Registrar nuevo cliente (usar `ICustomerRepository`).
   - `GET /api/customers/by-email?email=...` – Buscar cliente por email.

3. **SalesController**:
   - `POST /api/sales` – Realizar una venta. Recibir en el body: `{ productId, quantity, customerId? }`. Debe usar `IPlaceOrderUseCase` y garantizar que el stock se descuente transaccionalmente.
   - `GET /api/sales?startDate=...&endDate=...` – Consultar ventas por rango de fechas (usar `IFetchSalesByFilterUseCase`).

**Requisitos técnicos**:
- Configurar CORS para permitir peticiones desde `http://localhost:5173` (puerto típico de Vite para React).
- Usar inyección de dependencias para resolver los casos de uso y repositorios.
- Definir DTOs apropiados para las respuestas y solicitudes (evitar exponer entidades de dominio directamente).
- Implementar manejo global de excepciones con middleware que devuelva respuestas `Problem Details` (RFC 7807).
- Mantener compatibilidad con Native AOT (evitar reflexión, usar mapeo manual o DTOs simples).
</task>

<technical_requirements>
- **Estructura de carpetas** en WebAPI:
  - `Controllers/`
  - `DTOs/`
  - `Middleware/` (para manejo de errores)
  - `Program.cs` (configuración de servicios, CORS, Swagger)
- **Configuración**:
  - Leer cadena de conexión de `appsettings.json`.
  - Registrar los proyectos `Application` e `Infrastructure` como referencias.
- **Validaciones**: Usar `DataAnnotations` o FluentValidation para validar los DTOs de entrada.
- **Documentación**: Incluir Swagger/OpenAPI (útil para pruebas y documentación).
</technical_requirements>

<execution_workflow>
1. **CREAR** el proyecto WebAPI usando `dotnet new webapi`.
2. **AGREGAR** referencias a los proyectos `Application` e `Infrastructure`.
3. **CONFIGURAR** el contenedor de DI para registrar todos los servicios necesarios (repositorios, casos de uso).
4. **IMPLEMENTAR** los controladores con sus respectivos endpoints.
5. **DEFINIR** los DTOs y el mapeo entre entidades y DTOs (manual o con un mapeador simple).
6. **AGREGAR** middleware de manejo de errores global.
7. **CONFIGURAR** CORS para permitir el frontend en desarrollo.
8. **PROBAR** los endpoints con Swagger o Postman.
</execution_workflow>

<output_format>
- Código completo de los controladores (`ProductsController.cs`, `CustomersController.cs`, `SalesController.cs`).
- Código de los DTOs (al menos `ProductDto.cs`, `CustomerDto.cs`, `SaleRequestDto.cs`, `SaleResponseDto.cs`).
- Código del middleware `ErrorHandlingMiddleware.cs`.
- `Program.cs` configurado.
- Archivo `appsettings.json` con la cadena de conexión.
- Breve documentación de los endpoints disponibles.
</output_format>