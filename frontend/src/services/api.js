// Cambiado a HTTPS para evitar errores de Mixed Content en GitHub Pages
const BASE_URL = 'https://UtmMarket.somee.com/api';

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
    const response = await fetch(`${BASE_URL}${endpoint}`, config);
    
    // Si la respuesta es exitosa pero no tiene contenido (204 No Content)
    if (response.status === 204) {
      return null;
    }

    // Leemos el texto primero para verificar si está vacío
    const text = await response.text();
    let data = null;
    
    try {
      data = text ? JSON.parse(text) : null;
    } catch (e) {
      // Si no es JSON (es HTML de error o texto plano)
      throw new Error(`Respuesta no válida del servidor (${response.status}): ${text.substring(0, 100)}`);
    }

    if (response.ok) {
      return data;
    }

    const error = new Error(data?.detail || data?.title || `Error ${response.status}: ${text}`);
    error.status = response.status;
    error.data = data;
    throw error;
  } catch (err) {
    if (!err.status) {
      console.error('Network Error:', err);
    }
    throw err;
  }
};
