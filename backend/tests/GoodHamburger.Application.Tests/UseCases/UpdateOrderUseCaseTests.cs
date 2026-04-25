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

public class UpdateOrderUseCaseTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IUpdateOrderUseCase _useCase;

    public UpdateOrderUseCaseTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _menuRepository = Substitute.For<IMenuRepository>();
        _useCase = new UpdateOrderUseCase(_orderRepository, _menuRepository);
    }

    [Fact]
    public async Task Execute_ShouldUpdateOrder_WithNewItems()
    {
        var orderId = Guid.NewGuid();
        var existingOrder = new Order();
        existingOrder.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));
        existingOrder.CalculateTotal();

        var newItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var request = new UpdateOrderRequest
        {
            ItemIds = newItemIds
        };

        var newMenuItems = new List<MenuItem>
        {
            new MenuItem("X Bacon", 7.00m, ItemType.Sandwich) { Id = newItemIds[0] },
            new MenuItem("Fries", 2.00m, ItemType.Fries) { Id = newItemIds[1] },
            new MenuItem("Soft drink", 2.50m, ItemType.Drink) { Id = newItemIds[2] }
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);
        _menuRepository.GetByIdsAsync(Arg.Any<List<Guid>>()).Returns(newMenuItems);
        _orderRepository.UpdateAsync(Arg.Any<Order>()).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(orderId, request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Subtotal.Should().Be(11.50m);
        result.Data.DiscountPercentage.Should().Be(20);
        result.Data.Total.Should().Be(9.20m);
        await _orderRepository.Received(1).UpdateAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var request = new UpdateOrderRequest
        {
            ItemIds = new List<Guid> { Guid.NewGuid() }
        };

        _orderRepository.GetByIdAsync(orderId).Returns((Order?)null);

        var result = await _useCase.ExecuteAsync(orderId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrado");
        await _orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenMenuItemsNotFound()
    {
        var orderId = Guid.NewGuid();
        var existingOrder = new Order();
        existingOrder.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));

        var request = new UpdateOrderRequest
        {
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        var partialMenuItems = new List<MenuItem>
        {
            new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = request.ItemIds[0] }
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);
        _menuRepository.GetByIdsAsync(Arg.Any<List<Guid>>()).Returns(partialMenuItems);

        var result = await _useCase.ExecuteAsync(orderId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrados no menu");
        await _orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenUpdatedOrderHasNoSandwich()
    {
        var orderId = Guid.NewGuid();
        var existingOrder = new Order();
        existingOrder.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));

        var newItemIds = new List<Guid> { Guid.NewGuid() };
        var request = new UpdateOrderRequest
        {
            ItemIds = newItemIds
        };

        var newMenuItems = new List<MenuItem>
        {
            new MenuItem("Fries", 2.00m, ItemType.Fries) { Id = newItemIds[0] }
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);
        _menuRepository.GetByIdsAsync(Arg.Any<List<Guid>>()).Returns(newMenuItems);

        var result = await _useCase.ExecuteAsync(orderId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("pelo menos um sanduíche");
        await _orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>());
    }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenUpdatedOrderHasDuplicateItems()
    {
        var orderId = Guid.NewGuid();
        var existingOrder = new Order();
        existingOrder.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));

        var duplicateItemId = Guid.NewGuid();
        var request = new UpdateOrderRequest
        {
            ItemIds = new List<Guid> { duplicateItemId, duplicateItemId }
        };

        var newMenuItems = new List<MenuItem>
        {
            new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = duplicateItemId },
            new MenuItem("X Burger", 5.00m, ItemType.Sandwich) { Id = duplicateItemId }
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);
        _menuRepository.GetByIdsAsync(Arg.Any<List<Guid>>()).Returns(newMenuItems);

        var result = await _useCase.ExecuteAsync(orderId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("duplicad");
        await _orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>());
    }
}
