using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IRegisterUseCase
{
    Task<Result<AuthResponse>> ExecuteAsync(RegisterRequest request);
}
