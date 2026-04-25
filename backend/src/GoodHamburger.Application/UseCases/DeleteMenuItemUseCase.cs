using GoodHamburger.Application.Common;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class DeleteMenuItemUseCase : IDeleteMenuItemUseCase
{
    private readonly IMenuRepository _menuRepository;

    public DeleteMenuItemUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<bool>> ExecuteAsync(Guid id)
    {
        var menuItem = await _menuRepository.GetByIdAsync(id);
        
        if (menuItem == null)
            return Result<bool>.Failure("Item não encontrado.");

        await _menuRepository.DeleteAsync(id);

        return Result<bool>.Success(true);
    }
}
