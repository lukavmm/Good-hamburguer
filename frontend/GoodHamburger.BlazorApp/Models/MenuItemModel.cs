using System;

namespace GoodHamburger.BlazorApp.Models;

public class MenuItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Type { get; set; }
}
