using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs;

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Normal;
}
