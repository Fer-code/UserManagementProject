import { useEffect, useState } from "react";
import UserForm from "../components/UserForm";
import { userService } from "../services/UserService.js";

export default function UsersPage() {
  const [users, setUsers] = useState([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [showForm, setShowForm] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);

  async function loadUsers() {
    try {
      setLoading(true);
      setError("");

      const data = await userService.getAll();
      setUsers(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadUsers();
  }, []);

  function handleCreate() {
    setSelectedUser(null);
    setShowForm(true);
    setError("");
    setSuccess("");
  }

  function handleEdit(user) {
    setSelectedUser(user);
    setShowForm(true);
    setError("");
    setSuccess("");
  }

  function handleCancel() {
    setSelectedUser(null);
    setShowForm(false);
  }

  async function handleSave(model) {
    try {
      setSaving(true);
      setError("");
      setSuccess("");

      if (selectedUser) {
        await userService.update(selectedUser.id, model);
        setSuccess("Usuário atualizado com sucesso!");
      } else {
        await userService.create(model);
        setSuccess("Usuário cadastrado com sucesso!");
      }

      handleCancel();
      await loadUsers();
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(user) {
    const confirmed = window.confirm(
      `Deseja realmente excluir ${user.name}?`
    );

    if (!confirmed) return;

    try {
      setError("");
      setSuccess("");

      await userService.delete(user.id);
      setSuccess("Usuário excluído com sucesso!");

      await loadUsers();
    } catch (err) {
      setError(err.message);
    }
  }

  const filteredUsers = users.filter((user) => {
    const term = search.toLowerCase();

    return (
      (user.name ?? "").toLowerCase().includes(term) ||
      (user.email ?? "").toLowerCase().includes(term) ||
      (user.cpf ?? "").includes(term)
    );
  });

  function formatDate(date) {
    if (!date) return "-";

    const value = new Date(date);

    return Number.isNaN(value.getTime())
      ? "-"
      : value.toLocaleDateString("pt-BR");
  }

  return (
    <main className="container">
      <header className="page-header">
        <div>
          <p className="eyebrow">ADMINISTRAÇÃO</p>
          <h1>Gerenciamento de usuários</h1>
          <p className="subtitle">
            Gerencie os usuários cadastrados no sistema.
          </p>
        </div>

        <button className="btn-primary" onClick={handleCreate}>
          + Novo usuário
        </button>
      </header>

      {error && (
        <div className="alert alert-error" role="alert">
          {error}
        </div>
      )}

      {success && (
        <div className="alert alert-success" role="status">
          {success}
        </div>
      )}

      <section className="card">
        <div className="table-header">
          <div>
            <h2>Usuários cadastrados</h2>
            <p>{users.length} usuário(s) no sistema</p>
          </div>

          <input
            className="search-input"
            type="search"
            placeholder="Buscar por nome, e-mail ou CPF"
            aria-label="Buscar usuários"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Nome</th>
                <th>E-mail</th>
                <th>CPF</th>
                <th>Cadastro</th>
                <th>Status</th>
                <th>Ações</th>
              </tr>
            </thead>

            <tbody>
              {loading ? (
                <tr>
                  <td colSpan="6" className="empty-state">
                    Carregando usuários...
                  </td>
                </tr>
              ) : filteredUsers.length === 0 ? (
                <tr>
                  <td colSpan="6" className="empty-state">
                    Nenhum usuário encontrado.
                  </td>
                </tr>
              ) : (
                filteredUsers.map((user) => (
                  <tr key={user.id}>
                    <td className="user-name">{user.name}</td>
                    <td>{user.email}</td>
                    <td>{user.cpf}</td>
                    <td>{formatDate(user.createdAt)}</td>

                    <td>
                      <span className={`status ${user.status?.toLowerCase()}`}>
                        {user.status}
                      </span>
                    </td>

                    <td>
                      <div className="row-actions">
                        <button
                          className="btn-text"
                          onClick={() => handleEdit(user)}
                        >
                          Editar
                        </button>

                        <button
                          className="btn-text danger"
                          onClick={() => handleDelete(user)}
                        >
                          Excluir
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </section>

      {showForm && (
        <div className="modal-backdrop">
          <section
            className="modal"
            role="dialog"
            aria-modal="true"
            aria-labelledby="modal-title"
          >
            <div className="modal-header">
              <div>
                <h2 id="modal-title">
                  {selectedUser
                    ? "Editar usuário"
                    : "Cadastrar usuário"}
                </h2>

                <p>Preencha os dados abaixo.</p>
              </div>

              <button
                type="button"
                className="close-button"
                onClick={handleCancel}
                aria-label="Fechar formulário"
                disabled={saving}
              >
                ×
              </button>
            </div>

            <UserForm
              user={selectedUser}
              onSave={handleSave}
              onCancel={handleCancel}
              error={error}
              saving={saving}
            />
          </section>
        </div>
      )}
    </main>
  );
}