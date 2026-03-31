import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { productService } from '../services/productService';
import SplitText from '../components/design/SplitText';
import ShinyText from '../components/design/ShinyText';

const ProductsPage = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    productService.getAll()
      .then(data => {
        setProducts(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  }, []);

  const handleBuy = (product) => {
    navigate('/venta', { state: { product } });
  };

  if (loading) return <div className="app-content"><p>Cargando productos...</p></div>;
  if (error) return <div className="app-content"><p style={{ color: 'red' }}>Error: {error}</p></div>;

  return (
    <div className="app-content">
      <SplitText 
        text="UTM MARKET" 
        className="title-main"
        delay={80}
        duration={1}
      />
      <br />
      <ShinyText 
        text="Catálogo de Productos" 
        speed={3} 
        color="#ff6b35" 
        shineColor="#ffffff"
      />

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Marca</th>
            <th>SKU</th>
            <th>Precio</th>
            <th>Stock</th>
            <th>Acción</th>
          </tr>
        </thead>
        <tbody>
          {products.map(p => (
            <tr key={p.productID}>
              <td>{p.productID}</td>
              <td>{p.name}</td>
              <td>{p.brand}</td>
              <td>{p.sku}</td>
              <td>{p.price.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' })}</td>
              <td>{p.stock}</td>
              <td>
                <button className="btn" onClick={() => handleBuy(p)}>Comprar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ProductsPage;
