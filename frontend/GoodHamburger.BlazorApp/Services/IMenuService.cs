using GoodHamburger.BlazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodHamburger.BlazorApp.Services;

public interface IMenuService
{
    Task<List<MenuItemModel>> GetMenuAsync();
}
