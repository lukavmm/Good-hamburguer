using FluentAssertions;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.UseCases;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace GoodHamburger.Application.Tests.UseCases;

public class DeleteMenuItemUseCaseTests
{
    private readonly IMenuRepository _menuRepository;
    private readonly IDeleteMenuItemUseCase _useCase;

    public DeleteMenuItemUseCaseTests()
    {
        _menuRepository = Substitute.For<IMenuRepository>();
        _useCase = new DeleteMenuItemUseCase(_menuRepository);
    }

    [Fact]
    public async Task Execute_ShouldDeleteMenuItem_WhenExists()
    {
        var menuItemId = Guid.NewGuid();
        var existingItem = new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = menuItemId };

        _menuRepository.GetByIdAsync(menuItemId).Returns(existingItem);
        _menuRepository.DeleteAsync(menuItemId).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(menuItemId);

        result.IsSuccess.Should().BeTrue();
        await _menuRepository.Received(1).DeleteAsync(menuItemId);
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenMenuItemNotFound()
    {
        var menuItemId = Guid.NewGuid();

        _menuRepository.GetByIdAsync(menuItemId).Returns((MenuItem?)null);

        var result = await _useCase.ExecuteAsync(menuItemId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Item não encontrado.");
        await _menuRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
    }
}
