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

public class CreateMenuItemUseCaseTests
{
    private readonly IMenuRepository _menuRepository;
    private readonly ICreateMenuItemUseCase _useCase;

    public CreateMenuItemUseCaseTests()
    {
        _menuRepository = Substitute.For<IMenuRepository>();
        _useCase = new CreateMenuItemUseCase(_menuRepository);
    }

    [Fact]
    public async Task Execute_ShouldCreateMenuItem_WithValidData()
    {
        var request = new CreateMenuItemRequest
        {
            Name = "X Salad",
            Price = 6.50m,
            Type = ItemType.Sandwich
        };

        _menuRepository.AddAsync(Arg.Any<MenuItem>()).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("X Salad");
        result.Data.Price.Should().Be(6.50m);
        result.Data.Type.Should().Be(ItemType.Sandwich);
        await _menuRepository.Received(1).AddAsync(Arg.Any<MenuItem>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenNameIsEmpty()
    {
        var request = new CreateMenuItemRequest
        {
            Name = "",
            Price = 6.50m,
            Type = ItemType.Sandwich
        };

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Nome é obrigatório.");
        await _menuRepository.DidNotReceive().AddAsync(Arg.Any<MenuItem>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenPriceIsZeroOrNegative()
    {
        var request = new CreateMenuItemRequest
        {
            Name = "X Salad",
            Price = 0,
            Type = ItemType.Sandwich
        };

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Valor tem que ser maior que zero.");
        await _menuRepository.DidNotReceive().AddAsync(Arg.Any<MenuItem>());
    }
}
