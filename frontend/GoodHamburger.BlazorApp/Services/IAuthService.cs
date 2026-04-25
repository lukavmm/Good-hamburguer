using GoodHamburger.BlazorApp.Models;

namespace GoodHamburger.BlazorApp.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<string?> GetUsernameAsync();
    Task<string?> GetRoleAsync();
    Task<bool> IsAuthenticatedAsync();
}
