using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface ICreateMenuItemUseCase
{
    Task<Result<MenuItemResponse>> ExecuteAsync(CreateMenuItemRequest request);
}
