using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "O nome é obrigátorio.")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres.")]
        [MaxLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O email é obrigátorio.")]
        [MinLength(10, ErrorMessage = "O email deve ter no mínimo 10 caracteres.")]
        [MaxLength(80, ErrorMessage = "O email deve ter no máximo 180 caracteres.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
            ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigátoria.")]
        [MinLength(3, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [MaxLength(80, ErrorMessage = "A senha deve ter no máximo 30 caracteres.")]
        public string Password { get; set; }
    }
}