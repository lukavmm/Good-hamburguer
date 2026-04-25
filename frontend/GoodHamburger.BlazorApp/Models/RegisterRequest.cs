using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.BlazorApp.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    [MinLength(3, ErrorMessage = "Usuário deve ter no mínimo 3 caracteres")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(4, ErrorMessage = "Senha deve ter no mínimo 4 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
    public int Role { get; set; } // 0 = Normal, 1 = Admin
}

