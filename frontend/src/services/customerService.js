import { request } from './api';

export const customerService = {
  getAll: () => request('/customers'),
  getById: (id) => request(`/customers/${id}`),
  getByEmail: (email) => request(`/customers/by-email?email=${encodeURIComponent(email)}`),
  create: (data) => request('/customers', { body: data }),
  delete: (id) => request(`/customers/${id}`, { method: 'DELETE' }),
};
