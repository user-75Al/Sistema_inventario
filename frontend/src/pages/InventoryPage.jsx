import { useState, useEffect } from 'react';
import { productService } from '../services/productService';
import SplitText from '../components/design/SplitText';

const InventoryPage = () => {
  const [products, setProducts] = useState([]);
  const [threshold, setThreshold] = useState(10);
  const [loading, setLoading] = useState(false);
  
  // Formulario nuevo producto
  const [newP, setNewP] = useState({ name: '', sku: '', brand: '', price: 0, stock: 0 });

  const loadAll = () => {
    setLoading(true);
    productService.getAll().then(data => { setProducts(data); setLoading(false); });
  };

  const loadLowStock = () => {
    setLoading(true);
    productService.getLowStock(threshold).then(data => { setProducts(data); setLoading(false); });
  };

  useEffect(() => { loadAll(); }, []);

  const handleCreate = async (e) => {
    e.preventDefault();
    await productService.create(newP);
    setNewP({ name: '', sku: '', brand: '', price: 0, stock: 0 });
    loadAll();
  };

  const handleDelete = async (id) => {
    if (window.confirm("¿Seguro que desea eliminar este producto?")) {
      await productService.delete(id);
      loadAll();
    }
  };

  return (
    <div className="app-content">
      <SplitText text="GESTIÓN DE INVENTARIO" className="title-main" />
      
      <div className="form-container">
        <h3>Registrar Nuevo Producto (Opción 3)</h3>
        <form onSubmit={handleCreate} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
          <input className="input-field" placeholder="Nombre" value={newP.name} onChange={e => setNewP({...newP, name: e.target.value})} required />
          <input className="input-field" placeholder="SKU" value={newP.sku} onChange={e => setNewP({...newP, sku: e.target.value})} required />
          <input className="input-field" placeholder="Marca" value={newP.brand} onChange={e => setNewP({...newP, brand: e.target.value})} />
          <input className="input-field" type="number" placeholder="Precio" value={newP.price} onChange={e => setNewP({...newP, price: parseFloat(e.target.value)})} required />
          <input className="input-field" type="number" placeholder="Stock" value={newP.stock} onChange={e => setNewP({...newP, stock: parseInt(e.target.value)})} required />
          <button type="submit" className="btn">Guardar Producto</button>
        </form>
      </div>

      <div className="form-container" style={{ marginTop: '2rem' }}>
        <h3>Filtros y Alertas (Opción 9)</h3>
        <div style={{ display: 'flex', gap: '1rem', alignItems: 'center' }}>
          <input type="number" className="input-field" style={{ width: '100px' }} value={threshold} onChange={e => setThreshold(e.target.value)} />
          <button className="btn" onClick={loadLowStock}>Ver Stock Crítico</button>
          <button className="btn" style={{ background: '#444' }} onClick={loadAll}>Ver Todo</button>
        </div>
      </div>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Stock</th>
            <th>Acción</th>
          </tr>
        </thead>
        <tbody>
          {products.map(p => (
            <tr key={p.productID}>
              <td>{p.productID}</td>
              <td>{p.name}</td>
              <td style={{ color: p.stock <= threshold ? 'red' : 'white' }}>{p.stock}</td>
              <td>
                <button className="btn" style={{ background: '#d9534f' }} onClick={() => handleDelete(p.productID)}>Eliminar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default InventoryPage;
