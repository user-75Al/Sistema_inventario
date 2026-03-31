import { BrowserRouter as Router, Routes, Route, NavLink } from 'react-router-dom';
import ProductsPage from './pages/ProductsPage';
import InventoryPage from './pages/InventoryPage';
import CustomersManagementPage from './pages/CustomersManagementPage';
import SalePage from './pages/SalePage';
import CustomerFormPage from './pages/CustomerFormPage';
import SalesHistoryPage from './pages/SalesHistoryPage';
import Plasma from './components/design/Plasma';
import './styles/global.css';

function App() {
  return (
    <Router basename="/Sistema_inventario">
      <div className="plasma-bg">
        <Plasma 
          color="#ff6b35"
          speed={0.4}
          direction="forward"
          scale={1.2}
          opacity={0.6}
          mouseInteractive={true}
        />
      </div>

      <div className="app-content">
        <nav className="nav-bar">
          <NavLink to="/" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`} end>
            TIENDA
          </NavLink>
          <NavLink to="/inventario" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            INVENTARIO
          </NavLink>
          <NavLink to="/clientes" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            CLIENTES
          </NavLink>
          <NavLink to="/historial" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            HISTORIAL
          </NavLink>
        </nav>

        <Routes>
          <Route path="/" element={<ProductsPage />} />
          <Route path="/inventario" element={<InventoryPage />} />
          <Route path="/clientes" element={<CustomersManagementPage />} />
          <Route path="/venta" element={<SalePage />} />
          <Route path="/cliente/nuevo" element={<CustomerFormPage />} />
          <Route path="/historial" element={<SalesHistoryPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
