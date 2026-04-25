using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;

    public CreateOrderUseCase(IOrderRepository orderRepository, IMenuRepository menuRepository)
    {
        _orderRepository = orderRepository;
        _menuRepository = menuRepository;
    }

    public async Task<Result<OrderResponse>> ExecuteAsync(CreateOrderRequest request)
    {
        try
        {
            var menuItems = await _menuRepository.GetByIdsAsync(request.ItemIds);
            
            if (menuItems.Count() != request.ItemIds.Count)
                return Result<OrderResponse>.Failure("Um ou mais itens não encontrado no menu.");

            var order = new Order();

            foreach (var item in menuItems)
            {
                order.AddItem(item);
            }

            if (!order.IsValid())
                return Result<OrderResponse>.Failure("Pedido tem que conter ao menos um sanduíche.");

            order.CalculateTotal();
            await _orderRepository.AddAsync(order);

            return Result<OrderResponse>.Success(MapToResponse(order));
        }
        catch (InvalidOperationException ex)
        {
            return Result<OrderResponse>.Failure(ex.Message);
        }
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
