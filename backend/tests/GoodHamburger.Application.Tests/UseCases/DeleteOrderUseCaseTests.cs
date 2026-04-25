using FluentAssertions;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.UseCases;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace GoodHamburger.Application.Tests.UseCases;

public class DeleteOrderUseCaseTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeleteOrderUseCase _useCase;

    public DeleteOrderUseCaseTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _useCase = new DeleteOrderUseCase(_orderRepository);
    }

    [Fact]
    public async Task Execute_ShouldDeleteOrder_WhenExists()
    {
        var orderId = Guid.NewGuid();
        var order = new Order();
        order.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));

        _orderRepository.GetByIdAsync(orderId).Returns(order);
        _orderRepository.DeleteAsync(orderId).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(orderId);

        result.IsSuccess.Should().BeTrue();
        await _orderRepository.Received(1).DeleteAsync(orderId);
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();

        _orderRepository.GetByIdAsync(orderId).Returns((Order?)null);

        var result = await _useCase.ExecuteAsync(orderId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrado");
        await _orderRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
    }
}
