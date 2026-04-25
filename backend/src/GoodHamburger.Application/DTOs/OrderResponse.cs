using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs;

public class OrderResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public int DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
}

public class OrderItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ItemType Type { get; set; }
}
