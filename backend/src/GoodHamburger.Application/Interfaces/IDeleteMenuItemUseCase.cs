using GoodHamburger.Application.Common;

namespace GoodHamburger.Application.Interfaces;

public interface IDeleteMenuItemUseCase
{
    Task<Result<bool>> ExecuteAsync(Guid id);
}
