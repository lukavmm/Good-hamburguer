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

public class CreateOrderUseCaseTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly ICreateOrderUseCase _useCase;

    public CreateOrderUseCaseTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _menuRepository = Substitute.For<IMenuRepository>();
        _useCase = new CreateOrderUseCase(_orderRepository, _menuRepository);
    }

    [Fact]
    public async Task Execute_ShouldCreateOrder_WithValidItems()
    {
        var request = new CreateOrderRequest
        {
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        var menuItems = new List<MenuItem>
        {
            new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = request.ItemIds[0] },
            new MenuItem("Fries", 2.00m, ItemType.Fries) { Id = request.ItemIds[1] }
        };

        _menuRepository.GetByIdsAsync(Arg.Any<List<Guid>>()).Returns(menuItems);
        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Subtotal.Should().Be(7.00m);
        result.Data.Total.Should().Be(6.30m);
        await _orderRepository.Received(1).AddAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task Execute_ShouldFail_WhenNoSandwich()
    {
        var request = new CreateOrderRequest
        {
            ItemIds = new List<Guid> { Guid.NewGuid() }
        };

        var menuItems = new List<MenuItem>
        {
            new MenuItem("Fries", 2.00m, ItemType.Fries) { Id = request.ItemIds[0] }
        };

        _menuRepository.GetByIdsAsync(Arg.Any<List<Guid>>()).Returns(menuItems);

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("sanduíche");
        await _orderRepository.DidNotReceive().AddAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task Execute_ShouldFail_WhenItemNotFound()
    {
        var request = new CreateOrderRequest
        {
            ItemIds = new List<Guid> { Guid.NewGuid() }
        };

        _menuRepository.GetByIdsAsync(Arg.Any<List<Guid>>()).Returns(new List<MenuItem>());

        var result = await _useCase.ExecuteAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrado");
    }
}
