import { useEffect, useState } from "react";

const initialForm = {
  name: "",
  email: "",
  cpf: "",
  status: "active",
};

export default function UserForm({
  user,
  onSave,
  onCancel,
  saving,
  error
}) {
  const [form, setForm] = useState(initialForm);

  const message = typeof error === "string" ? error : error?.message;
  const details = error?.errors ? Object.values(error.errors).flat() : [];

  useEffect(() => {
    if (user) {
      setForm({
        name: user.name ?? "",
        email: user.email ?? "",
        cpf: user.cpf ?? "",
        status: user.status ?? "",
      });
    } else {
      setForm({ ...initialForm });
    }
  }, [user]);

  function handleChange(event) {
    const { name, value, type, checked } = event.target;

    setForm((previous) => ({
      ...previous,
      [name]: type === "checkbox" ? checked : value,
    }));
  }

  function handleSubmit(event) {
    event.preventDefault();

    const payload = {
      ...form,
      name: form.name.trim(),
      email: form.email.trim(),
      cpf: form.cpf.replace(/\D/g, ""),
    };

    if (!user) {
      delete payload.status;
    }

    onSave(payload);
  }
  
  return (
    <form onSubmit={handleSubmit} className="user-form">

      {message && (
        <div className="alert alert-error" role="alert">
          <strong>{message}</strong>
          {details.length > 0 && (
            <ul>
              {details.map((msg, i) => (
                <li key={i}>{msg}</li>
              ))}
            </ul>
          )}
        </div>
      )}

      <div className="form-group">
        <label htmlFor="name">Nome completo</label>
        <input
          id="name"
          name="name"
          value={form.name}
          onChange={handleChange}
          placeholder="Digite o nome completo"
          maxLength={150}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="email">E-mail</label>
        <input
          id="email"
          name="email"
          type="email"
          value={form.email}
          onChange={handleChange}
          placeholder="exemplo@email.com"
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="cpf">CPF</label>
        <input
          id="cpf"
          name="cpf"
          inputMode="numeric"
          value={form.cpf}
          onChange={handleChange}
          placeholder="xxx.xxx.xxx-xx"
          maxLength={11}
          pattern="[0-9]{11}"
          title="Informe os 11 dígitos do CPF"
          required
        />
      </div>

      {user && (
        <div className="form-group">
          <label htmlFor="status">Status</label>
          <select
            id="status"
            name="status"
            value={form.status}
            onChange={handleChange}
            required
          >
            <option value="active">Ativo</option>
            <option value="inactive">Inativo</option>
            <option value="blocked">Bloqueado</option>
          </select>
        </div>
      )}

      <div className="form-actions">
        <button
          type="button"
          className="btn-secondary"
          onClick={onCancel}
          disabled={saving}
        >
          Cancelar
        </button>

        <button
          type="submit"
          className="btn-primary"
          disabled={saving}
        >
          {saving
            ? "Salvando..."
            : user
              ? "Salvar alterações"
              : "Cadastrar usuário"}
        </button>
      </div>
    </form>
  );
}