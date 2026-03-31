import { useState, useEffect } from 'react';
import { saleService } from '../services/saleService';
import SplitText from '../components/design/SplitText';
import ShinyText from '../components/design/ShinyText';

const SalesHistoryPage = () => {
  const [sales, setSales] = useState([]);
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchSales = (start, end) => {
    setLoading(true);
    setError(null);
    saleService.getByDateRange(start, end)
      .then(data => {
        setSales(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchSales();
  }, []);

  const handleFilter = (e) => {
    e.preventDefault();
    fetchSales(startDate, endDate);
  };

  return (
    <div className="app-content">
      <SplitText text="HISTORIAL DE VENTAS" className="title-main" />
      <br />
      <ShinyText text="Filtra por fechas para consultar ventas" speed={3} color="#ff6b35" shineColor="#ffffff" />

      <div className="form-container" style={{ marginTop: '2rem' }}>
        <form onSubmit={handleFilter} style={{ display: 'flex', gap: '2rem', alignItems: 'flex-end' }}>
          <div>
            <label>Fecha Inicio:</label>
            <input 
              type="date" 
              className="input-field" 
              value={startDate} 
              onChange={(e) => setStartDate(e.target.value)}
            />
          </div>
          <div>
            <label>Fecha Fin:</label>
            <input 
              type="date" 
              className="input-field" 
              value={endDate} 
              onChange={(e) => setEndDate(e.target.value)}
            />
          </div>
          <button type="submit" className="btn" disabled={loading}>
            Filtrar
          </button>
        </form>
      </div>

      {loading ? (
        <p style={{ marginTop: '2rem' }}>Consultando historial...</p>
      ) : error ? (
        <p style={{ color: 'red', marginTop: '2rem' }}>Error: {error}</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Folio</th>
              <th>Fecha</th>
              <th>Cliente</th>
              <th>Total</th>
              <th>Acción</th>
            </tr>
          </thead>
          <tbody>
            {sales.map(s => (
              <tr key={s.saleId}>
                <td>{s.folio}</td>
                <td>{new Date(s.saleDate || Date.now()).toLocaleString()}</td>
                <td>{s.customerName || 'Público General'}</td>
                <td>{s.total.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' })}</td>
                <td>
                  <button className="btn" style={{ background: '#444' }}>Ver Detalle</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default SalesHistoryPage;
