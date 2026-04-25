namespace GoodHamburger.Application.DTOs;

public class CreateOrderRequest
{
    public List<Guid> ItemIds { get; set; } = new();
}
