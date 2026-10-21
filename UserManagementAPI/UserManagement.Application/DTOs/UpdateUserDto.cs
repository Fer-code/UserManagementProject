using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs
{
    public class UpdateUserDto
    {
        [StringLength(150, MinimumLength = 3)]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido.")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [RegularExpression(@"^\d{11}$")]
        public string? Cpf { get; set; }

        public string? Status { get; set; }
    }
}
