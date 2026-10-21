using UserManagement.Application.DTOs;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();

            return users.Select(ToDto);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return ToDto(user);
        }

        public async Task<User> CreateAsync(CreateUserDto dto)
        {
            // variaveis
            var name = dto.Name?.Trim();
            var email = dto.Email?.Trim();
            var cpf = dto.Cpf?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O email é obrigatório.");

            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("O CPF é obrigatório.");

            if (await _repository.EmailExistsAsync(email))
                throw new ConflictException("Este email já está cadastrado.");

            if (await _repository.CpfExistsAsync(cpf))
                throw new ConflictException("Este CPF já está cadastrado.");
            
            // Objeto da classe user
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                CreatedAt = DateTime.UtcNow,
                Status = "active"
            };

            await _repository.AddAsync(user);

            return user;
        }

        public async Task<User> UpdateUserAsync(Guid id, UpdateUserDto model)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            // Nome
            if (model.Name != null)
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    throw new ArgumentException("O nome não pode ser vazio.");

                user.Name = model.Name.Trim();
            }

            // Email
            if (model.Email != null)
            {
                if (string.IsNullOrWhiteSpace(model.Email))
                    throw new ArgumentException("O email não pode ser vazio.");

                var email = model.Email.Trim();

                if (await _repository.EmailExistsAsync(email, id))
                    throw new ConflictException("Este email já está cadastrado.");

                user.Email = email;
            }

            // CPF
            if (model.Cpf != null)
            {
                if (string.IsNullOrWhiteSpace(model.Cpf))
                    throw new ArgumentException("O CPF não pode ser vazio.");

                var cpf = model.Cpf.Trim();

                if (await _repository.CpfExistsAsync(cpf, id))
                    throw new ConflictException("Este CPF já está cadastrado.");

                user.Cpf = cpf;
            }

            // Status
            if (model.Status != null)
            {
                var status = model.Status.Trim().ToLowerInvariant();

                if (status != "active" && status != "inactive" && status != "blocked")
                    throw new ArgumentException(
                        "Status inválido. Utilize active, inactive ou blocked.");

                user.Status = status;
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(user);

            return user;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            await _repository.DeleteAsync(user);
        }

        private static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Status = user.Status
            };
        }
    }
}
