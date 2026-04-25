using FluentAssertions;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.UseCases;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace GoodHamburger.Application.Tests.UseCases;

public class GetAllOrdersUseCaseTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IGetAllOrdersUseCase _useCase;

    public GetAllOrdersUseCaseTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _useCase = new GetAllOrdersUseCase(_orderRepository);
    }

    [Fact]
    public async Task Execute_ShouldReturnAllOrders()
    {
        var order1 = new Order();
        order1.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));
        order1.CalculateTotal();

        var order2 = new Order();
        order2.AddItem(new MenuItem("X Egg", 4.50m, ItemType.Sandwich));
        order2.AddItem(new MenuItem("Fries", 2.00m, ItemType.Fries));
        order2.CalculateTotal();

        var orders = new List<Order> { order1, order2 };

        _orderRepository.GetAllAsync().Returns(orders);

        var result = await _useCase.ExecuteAsync();

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Count().Should().Be(2);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenNoOrders()
    {
        var orders = new List<Order>();

        _orderRepository.GetAllAsync().Returns(orders);

        var result = await _useCase.ExecuteAsync();

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Count().Should().Be(0);
    }
}
