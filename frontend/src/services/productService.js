import { request } from './api';

export const productService = {
  getAll: () => request('/products'),
  getById: (id) => request(`/products/${id}`),
  getLowStock: (threshold = 10) => request(`/products/low-stock?threshold=${threshold}`),
  create: (data) => request('/products', { body: data }),
  delete: (id) => request(`/products/${id}`, { method: 'DELETE' }),
};
