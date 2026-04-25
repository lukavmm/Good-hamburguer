using System;
using System.Collections.Generic;

namespace GoodHamburger.BlazorApp.Models;

public class OrderModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderItemModel> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public int DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
}

public class OrderItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Type { get; set; }
}
