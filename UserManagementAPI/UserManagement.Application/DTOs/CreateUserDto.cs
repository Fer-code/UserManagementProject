using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(150, MinimumLength = 3,
            ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$",
            ErrorMessage = "O CPF deve conter 11 números.")]
        public string Cpf { get; set; } = string.Empty;
    }
}
