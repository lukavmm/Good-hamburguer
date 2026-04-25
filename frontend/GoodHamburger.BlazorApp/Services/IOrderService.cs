using GoodHamburger.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodHamburger.BlazorApp.Services;

public interface IOrderService
{
    Task<OrderModel> CreateOrderAsync(List<Guid> itemIds);
    Task<List<OrderModel>> GetAllOrdersAsync();
    Task<OrderModel> GetOrderByIdAsync(Guid id);
    Task<OrderModel> UpdateOrderAsync(Guid id, List<Guid> itemIds);
    Task DeleteOrderAsync(Guid id);
}

