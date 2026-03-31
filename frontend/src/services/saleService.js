import { request } from './api';

export const saleService = {
  create: (data) => request('/sales', { body: data }),
  getByDateRange: (startDate, endDate) => {
    let query = '/sales';
    const params = [];
    if (startDate) params.push(`startDate=${startDate}`);
    if (endDate) params.push(`endDate=${endDate}`);
    if (params.length) query += `?${params.join('&')}`;
    return request(query);
  },
};
