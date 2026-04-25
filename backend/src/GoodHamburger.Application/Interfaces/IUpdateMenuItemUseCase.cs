using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IUpdateMenuItemUseCase
{
    Task<Result<MenuItemResponse>> ExecuteAsync(Guid id, UpdateMenuItemRequest request);
}
