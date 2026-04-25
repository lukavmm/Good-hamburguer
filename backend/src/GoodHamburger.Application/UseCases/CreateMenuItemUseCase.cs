using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class CreateMenuItemUseCase : ICreateMenuItemUseCase
{
    private readonly IMenuRepository _menuRepository;

    public CreateMenuItemUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<MenuItemResponse>> ExecuteAsync(CreateMenuItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<MenuItemResponse>.Failure("Nome é obrigatório.");

        if (request.Price <= 0)
            return Result<MenuItemResponse>.Failure("Valor tem que ser maior que zero.");

        var menuItem = new MenuItem(request.Name, request.Price, request.Type);

        await _menuRepository.AddAsync(menuItem);

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
