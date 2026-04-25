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

public class UpdateMenuItemUseCaseTests
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUpdateMenuItemUseCase _useCase;

    public UpdateMenuItemUseCaseTests()
    {
        _menuRepository = Substitute.For<IMenuRepository>();
        _useCase = new UpdateMenuItemUseCase(_menuRepository);
    }

    [Fact]
    public async Task Execute_ShouldUpdateMenuItem_WithValidData()
    {
        var menuItemId = Guid.NewGuid();
        var existingItem = new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = menuItemId };

        var request = new UpdateMenuItemRequest
        {
            Name = "X Burger Special",
            Price = 7.50m,
            Type = ItemType.Sandwich
        };

        _menuRepository.GetByIdAsync(menuItemId).Returns(existingItem);
        _menuRepository.UpdateAsync(Arg.Any<MenuItem>()).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(menuItemId, request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("X Burger Special");
        result.Data.Price.Should().Be(7.50m);
        await _menuRepository.Received(1).UpdateAsync(Arg.Any<MenuItem>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenMenuItemNotFound()
    {
        var menuItemId = Guid.NewGuid();
        var request = new UpdateMenuItemRequest
        {
            Name = "X Burger Special",
            Price = 7.50m,
            Type = ItemType.Sandwich
        };

        _menuRepository.GetByIdAsync(menuItemId).Returns((MenuItem?)null);

        var result = await _useCase.ExecuteAsync(menuItemId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Item não encontrado no menu.");
        await _menuRepository.DidNotReceive().UpdateAsync(Arg.Any<MenuItem>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenNameIsEmpty()
    {
        var menuItemId = Guid.NewGuid();
        var existingItem = new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = menuItemId };

        var request = new UpdateMenuItemRequest
        {
            Name = "",
            Price = 7.50m,
            Type = ItemType.Sandwich
        };

        _menuRepository.GetByIdAsync(menuItemId).Returns(existingItem);

        var result = await _useCase.ExecuteAsync(menuItemId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Nome é obrigatório.");
        await _menuRepository.DidNotReceive().UpdateAsync(Arg.Any<MenuItem>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenPriceIsZeroOrNegative()
    {
        var menuItemId = Guid.NewGuid();
        var existingItem = new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = menuItemId };

        var request = new UpdateMenuItemRequest
        {
            Name = "X Burger Special",
            Price = 0,
            Type = ItemType.Sandwich
        };

        _menuRepository.GetByIdAsync(menuItemId).Returns(existingItem);

        var result = await _useCase.ExecuteAsync(menuItemId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("O preço deve ser maior que zero.");
        await _menuRepository.DidNotReceive().UpdateAsync(Arg.Any<MenuItem>());
    }
}
