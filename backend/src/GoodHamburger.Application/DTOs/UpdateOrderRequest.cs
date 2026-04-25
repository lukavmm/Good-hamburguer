namespace GoodHamburger.Application.DTOs;

public class UpdateOrderRequest
{
    public List<Guid> ItemIds { get; set; } = new();
}
