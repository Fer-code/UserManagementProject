using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infra.Data;

namespace UserManagement.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            Guid? excludeUserId = null)
        {
            return await _context.Users.AnyAsync(u =>
                u.Email == email &&
                (!excludeUserId.HasValue || u.Id != excludeUserId.Value));
        }

        public async Task<bool> CpfExistsAsync(
            string cpf,
            Guid? excludeUserId = null)
        {
            return await _context.Users.AnyAsync(u =>
                u.Cpf == cpf &&
                (!excludeUserId.HasValue || u.Id != excludeUserId.Value));
        }
    }
}
