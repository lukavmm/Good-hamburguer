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

public class RegisterUseCaseTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRegisterUseCase _useCase;

    public RegisterUseCaseTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _useCase = new RegisterUseCase(_userRepository, _passwordHasher, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Execute_ShouldRegisterUser_WhenDataIsValid()
    {
        var request = new RegisterRequest
        {
            Username = "newuser",
            Password = "password123",
            Role = UserRole.Normal
        };

        var hashedPassword = "hashedPassword123";
        var expectedToken = "jwt-token-456";

        _userRepository.UsernameExistsAsync("newuser").Returns(false);
        _passwordHasher.HashPassword("password123").Returns(hashedPassword);
        _jwtTokenGenerator.GenerateToken(Arg.Any<User>()).Returns(expectedToken);
        _userRepository.AddAsync(Arg.Any<User>()).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Token.Should().Be(expectedToken);
        result.Data.Username.Should().Be("newuser");
        result.Data.Role.Should().Be("Normal");
        await _userRepository.Received(1).AddAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenUsernameAlreadyExists()
    {
        var request = new RegisterRequest
        {
            Username = "existinguser",
            Password = "password123",
            Role = UserRole.Normal
        };

        _userRepository.UsernameExistsAsync("existinguser").Returns(true);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Usuário já existe");
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenUsernameIsEmpty()
    {
        var request = new RegisterRequest
        {
            Username = "",
            Password = "password123",
            Role = UserRole.Normal
        };

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Usuário é obrigatório.");
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenPasswordIsEmpty()
    {
        var request = new RegisterRequest
        {
            Username = "newuser",
            Password = "",
            Role = UserRole.Normal
        };

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Senha é obrigatória.");
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenPasswordIsTooShort()
    {
        var request = new RegisterRequest
        {
            Username = "newuser",
            Password = "123",
            Role = UserRole.Normal
        };

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("pelo menos 4 caracteres");
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
    }
}
