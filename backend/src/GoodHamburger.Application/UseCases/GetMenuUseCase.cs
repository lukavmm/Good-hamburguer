using GoodHamburger.Application.Common;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class GetMenuUseCase : IGetMenuUseCase
{
    private readonly IMenuRepository _menuRepository;

    public GetMenuUseCase(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<Result<IEnumerable<MenuItemResponse>>> ExecuteAsync()
    {
        var menuItems = await _menuRepository.GetAllAsync();
        
        var response = menuItems.Select(m => new MenuItemResponse
        {
            Id = m.Id,
            Name = m.Name,
            Price = m.Price,
            Type = m.Type
        });

        return Result<IEnumerable<MenuItemResponse>>.Success(response);
    }
}
