using FluentAssertions;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.UseCases;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace GoodHamburger.Application.Tests.UseCases;

public class GetMenuItemByIdUseCaseTests
{
    private readonly IMenuRepository _menuRepository;
    private readonly IGetMenuItemByIdUseCase _useCase;

    public GetMenuItemByIdUseCaseTests()
    {
        _menuRepository = Substitute.For<IMenuRepository>();
        _useCase = new GetMenuItemByIdUseCase(_menuRepository);
    }

    [Fact]
    public async Task Execute_ShouldReturnMenuItem_WhenExists()
    {
        var menuItemId = Guid.NewGuid();
        var menuItem = new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = menuItemId };

        _menuRepository.GetByIdAsync(menuItemId).Returns(menuItem);

        var result = await _useCase.ExecuteAsync(menuItemId);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(menuItemId);
        result.Data.Name.Should().Be("X Burger");
        result.Data.Price.Should().Be(5.00m);
        result.Data.Type.Should().Be(ItemType.Sandwich);
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenMenuItemNotFound()
    {
        var menuItemId = Guid.NewGuid();

        _menuRepository.GetByIdAsync(menuItemId).Returns((MenuItem?)null);

        var result = await _useCase.ExecuteAsync(menuItemId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Item não encontrado no menu.");
    }
}
