const API_URL = import.meta.env.VITE_API_URL;

async function request(endpoint = "", options = {}) {
  const response = await fetch(`${API_URL}/users${endpoint}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  });

  const data = await response.json();

    if (!response.ok) {
        throw new Error(
            data.message ||
            data.detail ||
            data.title ||
            "Erro ao realizar a operação."
        );
    }

    console.log(data);

    return data;
}

// Consultando endpoints
export const userService = {
  getAll() {
    return request();
  },

  getById(id) {
    return request(`/${id}`);
  },

  create(user) {
    return request("", {
      method: "POST",
      body: JSON.stringify(user),
    });
  },

  update(id, user) {
    return request(`/${id}`, {
      method: "PATCH",
      body: JSON.stringify(user),
    });
  },

  delete(id) {
    return request(`/${id}`, {
      method: "DELETE",
    });
  },
};