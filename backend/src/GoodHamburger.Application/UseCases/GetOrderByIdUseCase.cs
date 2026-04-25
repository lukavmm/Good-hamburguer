using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class GetOrderByIdUseCase : IGetOrderByIdUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderResponse>> ExecuteAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
            return Result<OrderResponse>.Failure($"Pedido com ID {id} não encontrado.");

        return Result<OrderResponse>.Success(MapToResponse(order));
    }

    private static OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = order.Items.Select(i => new OrderItemResponse
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Type = i.Type
            }).ToList(),
            Subtotal = order.Subtotal,
            DiscountPercentage = order.DiscountPercentage,
            DiscountAmount = order.DiscountAmount,
            Total = order.Total
        };
    }
}
