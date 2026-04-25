using GoodHamburger.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodHamburger.BlazorApp.Services;

public interface IMenuItemManagementService
{
    Task<List<MenuItemModel>> GetAllAsync();
    Task<MenuItemModel> GetByIdAsync(Guid id);
    Task<MenuItemModel> CreateAsync(MenuItemModel item);
    Task<MenuItemModel> UpdateAsync(Guid id, MenuItemModel item);
    Task DeleteAsync(Guid id);
}
