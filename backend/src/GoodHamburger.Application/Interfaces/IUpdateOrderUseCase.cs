using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IUpdateOrderUseCase
{
    Task<Result<OrderResponse>> ExecuteAsync(Guid id, UpdateOrderRequest request);
}
