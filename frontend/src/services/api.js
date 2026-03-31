// Usamos un proxy para que GitHub Pages (https) pueda hablar con SomEE (http) sin bloqueos de seguridad
const BASE_URL = 'http://UtmMarket.somee.com/api';
const PROXY_URL = 'https://corsproxy.io/?';

export const request = async (endpoint, options = {}) => {
  const { body, ...customConfig } = options;
  
  const headers = {
    'Content-Type': 'application/json',
  };

  const config = {
    method: options.method || (body ? 'POST' : 'GET'),
    ...customConfig,
    headers: {
      ...headers,
      ...customConfig.headers,
    },
  };

  if (body) {
    config.body = JSON.stringify(body);
  }

  try {
    // Envolvemos la petición con el proxy seguro
    const finalUrl = `${PROXY_URL}${encodeURIComponent(BASE_URL + endpoint)}`;
    const response = await fetch(finalUrl, config);
    
    if (response.status === 204) {
      return null;
    }

    const text = await response.text();
    let data = null;
    
    try {
      data = text ? JSON.parse(text) : null;
    } catch (e) {
      throw new Error(`Respuesta no válida del servidor (${response.status})`);
    }

    if (response.ok) {
      return data;
    }

    const error = new Error(data?.detail || data?.title || `Error ${response.status}`);
    error.status = response.status;
    error.data = data;
    throw error;
  } catch (err) {
    console.error('Network Error:', err);
    throw err;
  }
};
