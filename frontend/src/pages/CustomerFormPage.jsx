import { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { customerService } from '../services/customerService';
import SplitText from '../components/design/SplitText';

const CustomerFormPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  
  const initialEmail = location.state?.email || '';
  const previousProduct = location.state?.product;
  const previousQuantity = location.state?.quantity;

  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState(initialEmail);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const customerId = await customerService.create({ fullName, email });
      const newCustomer = { customerID: customerId, fullName, email };
      
      // Redirigir a la venta con el cliente nuevo
      navigate('/venta', { 
        state: { 
          customer: newCustomer, 
          product: previousProduct, 
          quantity: previousQuantity 
        } 
      });
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="app-content">
      <SplitText text="REGISTRAR CLIENTE" className="title-main" />
      
      <div className="form-container">
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: '1rem' }}>
            <label>Nombre Completo:</label>
            <input 
              type="text" 
              className="input-field" 
              value={fullName} 
              onChange={(e) => setFullName(e.target.value)}
              required
            />
          </div>
          <div style={{ marginBottom: '1rem' }}>
            <label>Email:</label>
            <input 
              type="email" 
              className="input-field" 
              value={email} 
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div style={{ display: 'flex', gap: '1rem', marginTop: '2rem' }}>
            <button type="submit" className="btn" disabled={loading}>
              {loading ? 'Registrando...' : 'Registrar y Continuar Venta'}
            </button>
            <button type="button" className="btn" style={{ background: '#666' }} onClick={() => navigate('/venta', { state: { product: previousProduct, quantity: previousQuantity } })}>
              Cancelar
            </button>
          </div>
        </form>
        {error && <p style={{ color: 'red', marginTop: '1rem' }}>{error}</p>}
      </div>
    </div>
  );
};

export default CustomerFormPage;
