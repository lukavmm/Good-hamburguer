using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface ILoginUseCase
{
    Task<Result<AuthResponse>> ExecuteAsync(LoginRequest request);
}
