using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IGetOrderByIdUseCase
{
    Task<Result<OrderResponse>> ExecuteAsync(Guid id);
}
