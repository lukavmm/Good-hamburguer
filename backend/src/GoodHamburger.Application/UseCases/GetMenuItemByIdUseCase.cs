using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class GetMenuItemByIdUseCase : IGetMenuItemByIdUseCase
{
    private readonly IMenuRepository _menuRepository;

    public GetMenuItemByIdUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<MenuItemResponse>> ExecuteAsync(Guid id)
    {
        var menuItem = await _menuRepository.GetByIdAsync(id);
        
        if (menuItem == null)
            return Result<MenuItemResponse>.Failure("Item não encontrado no menu.");

        return Result<MenuItemResponse>.Success(MapToResponse(menuItem));
    }

    private static MenuItemResponse MapToResponse(MenuItem menuItem)
    {
        return new MenuItemResponse
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Price = menuItem.Price,
            Type = menuItem.Type
        };
    }
}
