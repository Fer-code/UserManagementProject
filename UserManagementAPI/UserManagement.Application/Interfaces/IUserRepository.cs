using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);

        Task<bool> EmailExistsAsync(
            string email,
            Guid? excludeUserId = null);

        Task<bool> CpfExistsAsync(
            string cpf,
            Guid? excludeUserId = null);
    }
}
