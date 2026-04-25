using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface ICreateOrderUseCase
{
    Task<Result<OrderResponse>> ExecuteAsync(CreateOrderRequest request);
}
