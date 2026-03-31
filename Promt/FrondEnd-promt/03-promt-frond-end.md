<role>
Actúa como un Desarrollador Frontend Senior experto en React 19 y JavaScript moderno. Tu tarea es crear una aplicación SPA (Single Page Application) que consuma la API REST de UtmMarket, la cual expone los casos de uso listados en la imagen proporcionada. Debes integrar los diseños de referencia (fondo y tipografías) y asegurar que la aplicación tenga las funcionalidades necesarias para gestionar productos, clientes y ventas.
</role>

<context>
- **Backend API**: Los siguientes casos de uso están implementados en el backend y disponibles a través de endpoints REST (asumir que la API sigue el patrón RESTful, ej. `GET /api/products`, `POST /api/sales`, etc.).  
  Lista de casos de uso (interfaces) que respaldan la funcionalidad:

 Lista de casos de uso (interfaces) que respaldan la funcionalidad:
@ICreateCustomerUseCase.cs
@ICreateProductUseCase.cs
@ICreateSaleUseCase.cs
@IDeleteCustomerUseCase.cs
@IDeleteProductUseCase.cs
@IFetchAllSalesUseCase.cs
@IFetchSaleByIdUseCase.cs
@IFetchSalesByFilterUseCase.cs
@IGetAllCustomersUseCase.cs
@IGetAllProductsUseCase.cs
@IGetProductByIdUseCase.cs
@ILowStockAlertUseCase.cs
@IPlaceOrderUseCase.cs
@IPlaceOrderWithCustomerUseCase.cs
@ISearchProductsUseCase.cs
@IUpdateProductStockUseCase.cs
@IUpdateProductUseCase.cs
@IUpdateSaleStatusUseCase.cs

Backend API http://localhost:5000
Frontend: React + vite
- **Diseños proporcionados** (deben ser copiados e integrados):
Archivos de diseño (contenido CSS y React):
  * @diseño-fondo.md (estilos de fondo)
  * @Tipografia-titulo.md (tipografía para títulos)
  * @tipografia.md (tipografía general)
- **Tecnologías**: React 19, JavaScript, Vite, `react-router-dom`, `fetch` nativo.
</context>

<task>
Generar el código completo de la aplicación React con la siguiente estructura y funcionalidades, consumiendo la API (que corre en `http://localhost:5000`) y aplicando los estilos de los archivos de referencia.
</task>

<technical_requirements>

### 1. Configuración inicial
- Crear proyecto con Vite: `npm create vite@latest frontend -- --template react`.
- Instalar dependencias: `react-router-dom` (para navegación) y cualquier otra necesaria (no se requieren librerías de UI adicionales).
- Configurar proxy en `vite.config.js` para redirigir `/api` a `http://localhost:5000/api`.

### 2. Estilos globales
- Crear archivo `src/styles/global.css` y copiar en él el contenido de los tres archivos de diseño (`diseño-fondo.md`, `Tipografia-titulo.md`, `tipografia.md`). Asegurar que las reglas CSS se apliquen correctamente (fondo en `body` o `#root`, tipografías en elementos correspondientes).
- Importar `global.css` en `src/main.jsx` (o en `App.jsx`).

### 3. Servicios API (carpeta `src/services/`)
- `api.js`: configuración base de fetch con manejo de errores común (usar `fetch` nativo).
- `productService.js`: funciones para obtener productos (`getAll`, `getById`, `getLowStock`) – basado en `IGetAllProductsUseCase`, `IGetProductByIdUseCase`, `ILowStockAlertUseCase`.
- `customerService.js`: funciones para obtener clientes (`getAll`, `getByEmail`), crear cliente (`create`) – basado en `IGetAllCustomersUseCase`, `ICreateCustomerUseCase`.
- `saleService.js`: funciones para crear venta (`create`), obtener ventas por fecha (`getByDateRange`) – basado en `IPlaceOrderUseCase` (o `IPlaceOrderWithCustomerUseCase`) y `IFetchSalesByFilterUseCase`.

### 4. Componentes y páginas (con estilos aplicados)
- **`pages/ProductsPage.jsx`**: Lista de productos en tabla (con columnas: ID, Nombre, Precio, Stock). Botón "Comprar" en cada fila que navegue a `/venta` pasando el producto seleccionado (usar `useNavigate` con state).
- **`pages/SalePage.jsx`**: Formulario para realizar venta. Debe:
- Mostrar producto seleccionado (nombre, precio).
- Campo para cantidad (validar >0 y <= stock).
- Campo para email de cliente (con búsqueda al enviar o con un botón "Buscar cliente").
- Si el email no existe (respuesta 404 de la API), mostrar mensaje y botón "Registrar nuevo cliente" que redirija a `/cliente/nuevo` con state que incluya el email y producto/cantidad.
- Al confirmar, llamar a `saleService.create` con los datos.
- **`pages/CustomerFormPage.jsx`**: Formulario para registrar nuevo cliente (nombre, email, teléfono). Al enviar, crear cliente vía API y luego redirigir a `/venta` con el cliente recién creado (usar state de navegación).
- **`pages/SalesHistoryPage.jsx`**: Filtro por fechas (inicio, fin) y tabla de resultados con columnas: Folio, Fecha, Total. Usar `saleService.getByDateRange`.
- **Componentes reutilizables** (opcionales pero recomendados):
- `Button.jsx`, `Input.jsx`, `Table.jsx` con estilos básicos.

### 5. Navegación (en `App.jsx` con `react-router-dom`)
- `/` → `ProductsPage`
- `/venta` → `SalePage`
- `/cliente/nuevo` → `CustomerFormPage`
- `/historial` → `SalesHistoryPage`

### 6. Manejo de estado
- Usar `useState` y `useEffect` en cada página.
- Para compartir datos entre rutas, usar `location.state` (pasado con `navigate`).

### 7. Flujo de cliente no existente
- Al intentar buscar cliente por email en `SalePage`, si la API responde con 404, mostrar opción "Registrar nuevo cliente".
- Al hacer clic, navegar a `/cliente/nuevo` con state que incluya el email y los datos de la venta en curso.
- Al completar el registro, redirigir de vuelta a `/venta` con el cliente nuevo y los mismos datos de producto/cantidad (usar state).

</technical_requirements>

<execution_workflow>
1. **CREAR** proyecto Vite + React.
2. **INSTALAR** dependencias (`react-router-dom`).
3. **COPIAR** estilos de los archivos de diseño a `src/styles/global.css` e importarlos en `main.jsx`.
4. **CONFIGURAR** proxy en `vite.config.js`.
5. **IMPLEMENTAR** servicios API en `src/services/`.
6. **CREAR** componentes reutilizables.
7. **DESARROLLAR** las páginas según especificaciones.
8. **PROBAR** flujo completo con backend real.
</execution_workflow>

<output_format>
- Código completo de todos los archivos frontend (estructura de carpetas y archivos), incluyendo:
- `src/styles/global.css` (con contenido de los archivos de diseño).
- `src/services/*.js`
- `src/components/*.jsx`
- `src/pages/*.jsx`
- `App.jsx`, `main.jsx`, `vite.config.js`, `package.json`.
- Breve guía de ejecución.
</output_format>

<quality_gate>
- ¿La aplicación muestra correctamente los estilos de fondo y tipografías?
- ¿Las páginas consumen la API según los casos de uso listados?
- ¿El flujo de cliente no existente redirige al registro y vuelve?
- ¿El proxy funciona y las peticiones se resuelven correctamente?
- ¿El código es limpio y está bien estructurado?
</quality_gate>