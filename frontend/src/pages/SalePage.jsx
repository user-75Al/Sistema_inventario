import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { customerService } from '../services/customerService';
import { saleService } from '../services/saleService';
import SplitText from '../components/design/SplitText';

const SalePage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const productFromState = location.state?.product;
  const quantityFromState = location.state?.quantity || 1;
  const customerFromState = location.state?.customer;

  const [product, setProduct] = useState(productFromState);
  const [quantity, setQuantity] = useState(quantityFromState);
  const [email, setEmail] = useState(customerFromState?.email || '');
  const [customer, setCustomer] = useState(customerFromState || null);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const [searchingCustomer, setSearchingCustomer] = useState(false);

  useEffect(() => {
    if (!product) {
      navigate('/');
    }
  }, [product, navigate]);

  const handleSearchCustomer = async (e) => {
    e.preventDefault();
    if (!email) return;
    
    setSearchingCustomer(true);
    setError(null);
    try {
      const data = await customerService.getByEmail(email);
      setCustomer(data);
    } catch (err) {
      if (err.status === 404) {
        setError("Cliente no encontrado.");
      } else {
        setError(err.message);
      }
    } finally {
      setSearchingCustomer(false);
    }
  };

  const handleRegisterRedirect = () => {
    navigate('/cliente/nuevo', { 
      state: { 
        email, 
        product, 
        quantity 
      } 
    });
  };

  const handleSubmitSale = async (e) => {
    e.preventDefault();
    if (!customer) {
      setError("Debe asociar un cliente antes de continuar.");
      return;
    }

    if (quantity > product.stock) {
      setError(`Stock insuficiente. Disponible: ${product.stock}`);
      return;
    }

    try {
      const result = await saleService.create({
        productId: product.productID,
        quantity: parseInt(quantity),
        customerId: customer.customerID
      });

      if (result.success) {
        setSuccess(`¡Venta realizada! Folio: ${result.folio}`);
        // Limpiar después de éxito
        setTimeout(() => navigate('/historial'), 3000);
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError(err.message);
    }
  };

  if (!product) return null;

  return (
    <div className="app-content">
      <SplitText text="PROCESAR VENTA" className="title-main" />

      <div className="form-container">
        <h3>Detalles del Producto</h3>
        <p><strong>Producto:</strong> {product.name} ({product.brand})</p>
        <p><strong>Precio:</strong> {product.price.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' })}</p>
        <p><strong>Stock Disponible:</strong> {product.stock}</p>
        
        <div style={{ marginTop: '1rem' }}>
          <label>Cantidad:</label>
          <input 
            type="number" 
            className="input-field" 
            value={quantity} 
            min="1" 
            max={product.stock}
            onChange={(e) => setQuantity(e.target.value)}
          />
        </div>
      </div>

      <div className="form-container">
        <h3>Datos del Cliente</h3>
        <form onSubmit={handleSearchCustomer} style={{ display: 'flex', gap: '1rem', alignItems: 'flex-end' }}>
          <div style={{ flex: 1 }}>
            <label>Email del Cliente:</label>
            <input 
              type="email" 
              className="input-field" 
              value={email} 
              onChange={(e) => setEmail(e.target.value)}
              placeholder="buscar@correo.com"
              required
            />
          </div>
          <button type="submit" className="btn" disabled={searchingCustomer}>
            {searchingCustomer ? 'Buscando...' : 'Buscar'}
          </button>
        </form>

        {error && error.includes("no encontrado") && (
          <div style={{ marginTop: '1rem', color: '#ffcc00' }}>
            <p>{error}</p>
            <button className="btn" style={{ background: '#444' }} onClick={handleRegisterRedirect}>
              Registrar Nuevo Cliente
            </button>
          </div>
        )}

        {customer && (
          <div style={{ marginTop: '1rem', color: '#00ff00' }}>
            <p><strong>Cliente Asociado:</strong> {customer.fullName}</p>
          </div>
        )}
      </div>

      <div style={{ marginTop: '2rem', display: 'flex', gap: '1rem' }}>
        <button className="btn" onClick={handleSubmitSale} disabled={!customer}>
          Confirmar Venta
        </button>
        <button className="btn" style={{ background: '#666' }} onClick={() => navigate('/')}>
          Cancelar
        </button>
      </div>

      {error && !error.includes("no encontrado") && <p style={{ color: 'red', marginTop: '1rem' }}>{error}</p>}
      {success && <p style={{ color: '#00ff00', marginTop: '1rem', fontSize: '1.2rem' }}>{success}</p>}
    </div>
  );
};

export default SalePage;
