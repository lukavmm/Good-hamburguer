using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class UpdateMenuItemUseCase : IUpdateMenuItemUseCase
{
    private readonly IMenuRepository _menuRepository;

    public UpdateMenuItemUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<MenuItemResponse>> ExecuteAsync(Guid id, UpdateMenuItemRequest request)
    {
        var menuItem = await _menuRepository.GetByIdAsync(id);
        
        if (menuItem == null)
            return Result<MenuItemResponse>.Failure("Item não encontrado no menu.");

        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<MenuItemResponse>.Failure("Nome é obrigatório.");

        if (request.Price <= 0)
            return Result<MenuItemResponse>.Failure("O preço deve ser maior que zero.");

        menuItem.Update(request.Name, request.Price, request.Type);

        await _menuRepository.UpdateAsync(menuItem);

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
