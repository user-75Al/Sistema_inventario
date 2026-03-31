<role>
Actúa como un Arquitecto de Software experto en Clean Architecture y desarrollo con .NET 10 y C# 14. Tu especialidad es diagnosticar problemas de integración con base de datos, diseñar casos de uso transaccionales y modificar esquemas de base de datos existentes respetando Native AOT y evitando reflexión.
</role>

<context>
Proyecto: **UtmMarket** – aplicación de consola en .NET 10 con Clean Architecture.
Stack Técnico: .NET 10, C# 14, Microsoft.Data.SqlClient.
Base de datos: SQL Server 2022 Express con tablas `Producto`, `Venta`, `DetalleVenta` y `Cliente` (creadas por scripts anteriores). La tabla `Venta` actualmente no tiene una columna que relacione la venta con un cliente; esa relación debería estar en `DetalleVenta` o en `Venta`. Por simplicidad, asumimos que queremos agregar una columna `ClienteID` en `Venta` (FK a `Cliente`). Si ya existe, se usará; si no, se debe agregar.

Estado actual:
- El menú de la consola tiene una opción 10 actualmente (debe ser eliminada).
- Existe una opción 6 para registrar nuevos clientes (asumir que está implementada correctamente).
- Se requiere una nueva funcionalidad que permita realizar una venta/apartado de productos asociándola a un cliente.
- Los datos deben persistir en las tablas correspondientes (`Venta`, `DetalleVenta`, y actualizar `Producto.Stock`).
- **Problema reportado**: Al intentar usar la nueva funcionalidad, al ingresar un correo de un cliente existente, el sistema no lo encuentra y pide registrarlo. Esto sugiere un error en la conexión a la base de datos, en la consulta de cliente por email, o en la configuración de la cadena de conexión.
- Además, se necesita modificar la consulta de ventas por fecha (probablemente la opción existente) para que también muestre si la venta está asociada a un cliente (nombre o email del cliente).

</context>

<task>
1. **Diagnosticar y corregir** el problema de conexión/consulta de cliente por email.
2. **Modificar la tabla `Venta`** para agregar una columna `ClienteID` (INT, FK a `Cliente(ClienteID)`), si no existe ya. Si la relación ya está modelada en `DetalleVenta` o en otra forma, ajustar según la estructura real.
3. **Modificar la opción de menú**: eliminar la opción 10 actual y reemplazarla con una nueva opción "10. Realizar venta/apartado de productos" que implemente el flujo completo descrito.
4. **Modificar la opción de consulta de ventas por fecha** (probablemente opción existente) para que incluya información del cliente (nombre y/o email) asociado a cada venta.
</task>

<technical_requirements>

### 1. Diagnóstico y corrección del problema de cliente por email
- Revisar la cadena de conexión en `appsettings.json` (o donde se configure) para asegurar que apunta a la base de datos correcta.
- Verificar la implementación de `ICustomerRepository.GetByEmailAsync(string email)`. Debe usar `SqlCommand` con parámetro y `SqlDataReader` para mapear a `Customer`. Asegurar que el email se pasa correctamente y que la consulta SQL es la adecuada.
- Si el método retorna `null` para un email existente, revisar si hay problemas de mayúsculas/minúsculas o espacios en blanco. Usar `LTRIM(RTRIM(email))` en SQL o `Trim()` en C#.
- Asegurar que el repositorio esté correctamente registrado en DI y que la conexión se abra/cierre adecuadamente.

### 2. Modificación de la tabla Venta (Base de Datos)
- **Si no existe la columna `ClienteID` en `Venta`**, agregarla:
  ```sql
  ALTER TABLE Venta ADD ClienteID INT NULL;
  ALTER TABLE Venta ADD CONSTRAINT FK_Venta_Cliente FOREIGN KEY (ClienteID) REFERENCES Cliente(ClienteID);

