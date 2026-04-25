using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class RegisterUseCase : IRegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponse>> ExecuteAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return Result<AuthResponse>.Failure("Usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result<AuthResponse>.Failure("Senha é obrigatória.");

        if (request.Password.Length < 4)
            return Result<AuthResponse>.Failure("A senha deve ter pelo menos 4 caracteres.");

        if (await _userRepository.UsernameExistsAsync(request.Username))
            return Result<AuthResponse>.Failure("Usuário já existe.");

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new User(request.Username, passwordHash, request.Role);

        await _userRepository.AddAsync(user);

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
