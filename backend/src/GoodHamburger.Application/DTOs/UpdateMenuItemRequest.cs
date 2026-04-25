using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs;

public class UpdateMenuItemRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ItemType Type { get; set; }
}
