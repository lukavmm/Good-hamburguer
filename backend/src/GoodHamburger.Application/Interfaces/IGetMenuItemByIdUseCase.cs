using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IGetMenuItemByIdUseCase
{
    Task<Result<MenuItemResponse>> ExecuteAsync(Guid id);
}
