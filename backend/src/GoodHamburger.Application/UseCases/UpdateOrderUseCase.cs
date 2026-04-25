using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class UpdateOrderUseCase : IUpdateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;

    public UpdateOrderUseCase(IOrderRepository orderRepository, IMenuRepository menuRepository)
    {
        _orderRepository = orderRepository;
        _menuRepository = menuRepository;
    }

    public async Task<Result<OrderResponse>> ExecuteAsync(Guid id, UpdateOrderRequest request)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            
            if (order == null)
                return Result<OrderResponse>.Failure($"Pedido com ID {id} não encontrado.");

            var menuItems = await _menuRepository.GetByIdsAsync(request.ItemIds);
            
            if (menuItems.Count() != request.ItemIds.Count)
                return Result<OrderResponse>.Failure("Um ou mais itens não encontrados no menu.");

            order.ClearItems();

            foreach (var item in menuItems)
            {
                order.AddItem(item);
            }

            if (!order.IsValid())
                return Result<OrderResponse>.Failure("O pedido deve conter pelo menos um sanduíche.");

            order.CalculateTotal();
            await _orderRepository.UpdateAsync(order);

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
