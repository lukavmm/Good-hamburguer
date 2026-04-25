using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class GetAllOrdersUseCase : IGetAllOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<IEnumerable<OrderResponse>>> ExecuteAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        var response = orders.Select(MapToResponse);
        return Result<IEnumerable<OrderResponse>>.Success(response);
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
