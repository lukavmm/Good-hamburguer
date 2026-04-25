using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IGetMenuUseCase
{
    Task<Result<IEnumerable<MenuItemResponse>>> ExecuteAsync();
}
