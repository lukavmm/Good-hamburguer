using FluentAssertions;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.UseCases;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace GoodHamburger.Application.Tests.UseCases;

public class GetOrderByIdUseCaseTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IGetOrderByIdUseCase _useCase;

    public GetOrderByIdUseCaseTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _useCase = new GetOrderByIdUseCase(_orderRepository);
    }

    [Fact]
    public async Task Execute_ShouldReturnOrder_WhenExists()
    {
        var orderId = Guid.NewGuid();
        var order = new Order();
        order.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));
        order.AddItem(new MenuItem("Fries", 2.00m, ItemType.Fries));
        order.CalculateTotal();

        _orderRepository.GetByIdAsync(orderId).Returns(order);

        var result = await _useCase.ExecuteAsync(orderId);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Subtotal.Should().Be(7.00m);
        result.Data.Total.Should().Be(6.30m);
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();

        _orderRepository.GetByIdAsync(orderId).Returns((Order?)null);

        var result = await _useCase.ExecuteAsync(orderId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrado");
    }
}
