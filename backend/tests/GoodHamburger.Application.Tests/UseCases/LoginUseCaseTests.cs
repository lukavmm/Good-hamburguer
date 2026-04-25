using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.UseCases;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace GoodHamburger.Application.Tests.UseCases;

public class LoginUseCaseTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILoginUseCase _useCase;

    public LoginUseCaseTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _useCase = new LoginUseCase(_userRepository, _passwordHasher, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Execute_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var request = new LoginRequest
        {
            Username = "admin",
            Password = "admin"
        };

        var user = new User("admin", "hashedPassword", UserRole.Admin);
        var expectedToken = "jwt-token-123";

        _userRepository.GetByUsernameAsync("admin").Returns(user);
        _passwordHasher.VerifyPassword("admin", "hashedPassword").Returns(true);
        _jwtTokenGenerator.GenerateToken(user).Returns(expectedToken);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Token.Should().Be(expectedToken);
        result.Data.Username.Should().Be("admin");
        result.Data.Role.Should().Be("Admin");
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenUserNotFound()
    {
        var request = new LoginRequest
        {
            Username = "nonexistent",
            Password = "password"
        };

        _userRepository.GetByUsernameAsync("nonexistent").Returns((User?)null);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("inválid");
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenPasswordIsInvalid()
    {
        var request = new LoginRequest
        {
            Username = "admin",
            Password = "wrongpassword"
        };

        var user = new User("admin", "hashedPassword", UserRole.Admin);

        _userRepository.GetByUsernameAsync("admin").Returns(user);
        _passwordHasher.VerifyPassword("wrongpassword", "hashedPassword").Returns(false);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Usuário ou senha inválidos.");
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenUsernameIsEmpty()
    {
        var request = new LoginRequest
        {
            Username = "",
            Password = "password"
        };

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Usuário é obrigatório.");
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenPasswordIsEmpty()
    {
        var request = new LoginRequest
        {
            Username = "admin",
            Password = ""
        };

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Senha é obrigatória.");
    }
}
