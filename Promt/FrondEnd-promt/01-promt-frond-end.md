<role>
Actúa como un Arquitecto de Software experto en .NET 10, C# 14 y Clean Architecture. Tu tarea es reestructurar el proyecto UtmMarket para convertirlo en un backend listo para servir una API REST, eliminando toda la lógica de consola y preparando la inyección de dependencias para ser usada desde una WebAPI.
</role>

<context>
- **Proyecto**: UtmMarket – actualmente una aplicación de consola en .NET 10 con Clean Architecture.
- **Estructura actual**:
  - `src/Core/` (Entidades, Repositorios, Casos de Uso)
  - `src/Application/` (Implementaciones de casos de uso)
  - `src/Infrastructure/` (Repositorios concretos, mapeo manual)
  - `Program.cs` (contiene el menú interactivo y la configuración de DI)
- **Objetivo**: Eliminar todo lo relacionado con la interfaz de consola y dejar el backend listo para ser referenciado por un proyecto WebAPI.
</context>

<task>
1. **Eliminar el menú de consola**:
   - Reemplazar el contenido de `Program.cs` para que solo contenga la configuración de servicios (DI) y no tenga lógica de interacción con el usuario.
   - Mantener la configuración de `HostBuilder` o `ServiceCollection` para que pueda ser reutilizada por una WebAPI.
2. **Asegurar que los casos de uso y repositorios sean fácilmente inyectables** en un proyecto WebAPI.
3. **Verificar que todas las referencias a `Console` (ReadLine, WriteLine) sean eliminadas** de cualquier capa (solo deben estar en la capa de presentación, que ahora será WebAPI).
4. **Mantener intacta toda la lógica de negocio** (casos de uso, repositorios, entidades, validaciones, transaccionalidad).
</task>

<technical_requirements>
- El nuevo `Program.cs` debe configurar los servicios usando `IServiceCollection` (similar a como se haría en una WebAPI).
- Opcionalmente, crear una clase estática `DependencyInjection` en `Application` o `Infrastructure` para centralizar el registro de servicios.
- Asegurar que las cadenas de conexión y configuraciones se lean de `appsettings.json` (preparar el terreno para WebAPI).
- El proyecto debe seguir compilando sin errores después de los cambios.
</technical_requirements>

<execution_workflow>
1. **IDENTIFICAR** y eliminar todas las ocurrencias de `Console.WriteLine`, `Console.ReadLine`, etc., en `Program.cs` y cualquier otra clase que no sea de presentación.
2. **REEMPLAZAR** la lógica de menú por una simple configuración de servicios.
3. **CREAR** (si no existe) un archivo `appsettings.json` en la raíz del proyecto con la cadena de conexión.
4. **VERIFICAR** que la inyección de dependencias siga funcionando ejecutando una compilación y, si es posible, una prueba simple (por ejemplo, resolviendo un caso de uso manualmente).
</execution_workflow>

<output_format>
- Código del nuevo `Program.cs`.
- (Opcional) Clase `DependencyInjection.cs` si se decide crear.
- Breve explicación de los cambios realizados y cómo probar que la configuración de servicios sigue funcionando.
</output_format>