import { useState, useEffect } from 'react';
import { customerService } from '../services/customerService';
import SplitText from '../components/design/SplitText';

const CustomersManagementPage = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(false);
  
  const loadAll = () => {
    setLoading(true);
    customerService.getAll().then(data => { setCustomers(data); setLoading(false); });
  };

  useEffect(() => { loadAll(); }, []);

  const handleDelete = async (id) => {
    if (window.confirm("¿Seguro que desea eliminar este cliente?")) {
      await customerService.delete(id);
      loadAll();
    }
  };

  return (
    <div className="app-content">
      <SplitText text="GESTIÓN DE CLIENTES" className="title-main" />
      
      <div className="form-container">
        <h3>Alta de Nuevo Cliente (Opción 6)</h3>
        <button className="btn" onClick={() => window.location.href='/cliente/nuevo'}>Abrir Formulario de Alta</button>
      </div>

      <div className="form-container" style={{ marginTop: '2rem' }}>
        <h3>Listado General (Opción 7)</h3>
        <button className="btn" style={{ background: '#444' }} onClick={loadAll}>Refrescar Lista</button>
      </div>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre Completo</th>
            <th>Email</th>
            <th>Acción</th>
          </tr>
        </thead>
        <tbody>
          {customers.map(c => (
            <tr key={c.customerID}>
              <td>{c.customerID}</td>
              <td>{c.fullName}</td>
              <td>{c.email}</td>
              <td>
                <button className="btn" style={{ background: '#d9534f' }} onClick={() => handleDelete(c.customerID)}>Eliminar (Opción 8)</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CustomersManagementPage;
