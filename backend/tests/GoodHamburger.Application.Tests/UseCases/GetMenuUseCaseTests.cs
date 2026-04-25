using FluentAssertions;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.UseCases;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace GoodHamburger.Application.Tests.UseCases;

public class GetMenuUseCaseTests
{
    private readonly IMenuRepository _menuRepository;
    private readonly IGetMenuUseCase _useCase;

    public GetMenuUseCaseTests()
    {
        _menuRepository = Substitute.For<IMenuRepository>();
        _useCase = new GetMenuUseCase(_menuRepository);
    }

    [Fact]
    public async Task Execute_ShouldReturnAllMenuItems()
    {
        var menuItems = new List<MenuItem>
        {
            new MenuItem("X Burger", 5.00m, ItemType.Sandwich),
            new MenuItem("X Egg", 4.50m, ItemType.Sandwich),
            new MenuItem("X Bacon", 7.00m, ItemType.Sandwich),
            new MenuItem("Fries", 2.00m, ItemType.Fries),
            new MenuItem("Soft drink", 2.50m, ItemType.Drink)
        };

        _menuRepository.GetAllAsync().Returns(menuItems);

        var result = await _useCase.ExecuteAsync();

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Count().Should().Be(5);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenNoMenuItems()
    {
        var menuItems = new List<MenuItem>();

        _menuRepository.GetAllAsync().Returns(menuItems);

        var result = await _useCase.ExecuteAsync();

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Count().Should().Be(0);
    }
}
