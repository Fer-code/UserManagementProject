const API_URL = import.meta.env.VITE_API_URL;

export class ApiError extends Error {
  constructor(message, errors = null) {
    super(message);
    this.name = "ApiError";
    this.errors = errors;
  }
}

function buildMessage(data) {
  if (data?.errors && typeof data.errors === "object") {
    const messages = Object.values(data.errors).flat();
    if (messages.length > 0) return messages.join(" ");
  }

  return (
    data?.message ||
    data?.detail ||
    data?.title ||
    "Erro ao realizar a operação."
  );
}

async function request(endpoint = "", options = {}) {
  const response = await fetch(`${API_URL}/users${endpoint}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  });

  // Lê como texto: tolera 204 (corpo vazio) e respostas que não são JSON
  const text = await response.text();

  let data = null;
  if (text) {
    try {
      data = JSON.parse(text);
    } catch {
      data = { message: text };
    }
  }

  if (!response.ok) {
    throw new ApiError(buildMessage(data), data?.errors ?? null);
  }

  return data;
}

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