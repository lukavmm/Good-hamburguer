using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponse>> ExecuteAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return Result<AuthResponse>.Failure("Usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result<AuthResponse>.Failure("Senha é obrigatória.");

        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
            return Result<AuthResponse>.Failure("Usuário inválido.");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("Usuário ou senha inválidos.");

        var token = _jwtTokenGenerator.GenerateToken(user);

        var response = new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role.ToString()
        };

        return Result<AuthResponse>.Success(response);
    }
}
