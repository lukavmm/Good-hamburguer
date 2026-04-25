using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IGetAllOrdersUseCase
{
    Task<Result<IEnumerable<OrderResponse>>> ExecuteAsync();
}
