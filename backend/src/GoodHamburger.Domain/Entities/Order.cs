using System;
using System.Collections.Generic;
using System.Linq;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    private readonly List<MenuItem> _items = new();
    public IReadOnlyCollection<MenuItem> Items => _items.AsReadOnly();

    public decimal Subtotal { get; private set; }
    public int DiscountPercentage { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal Total { get; private set; }

    public Order()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(MenuItem item)
    {
        if (_items.Any(i => i.Id == item.Id))
            throw new InvalidOperationException("Não é possível adicionar itens duplicados ao pedido.");

        if (item.Type == ItemType.Sandwich && _items.Any(i => i.Type == ItemType.Sandwich))
            throw new InvalidOperationException("Um pedido pode conter apenas um sanduíche.");

        _items.Add(item);
        Subtotal = _items.Sum(i => i.Price);
    }

    public void ClearItems()
    {
        _items.Clear();
        Subtotal = 0;
        DiscountPercentage = 0;
        DiscountAmount = 0;
        Total = 0;
    }

    public void CalculateTotal()
    {
        Subtotal = _items.Sum(i => i.Price);
        DiscountPercentage = CalculateDiscountPercentage();
        DiscountAmount = Subtotal * DiscountPercentage / 100;
        Total = Subtotal - DiscountAmount;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsValid()
    {
        return _items.Any(i => i.Type == ItemType.Sandwich);
    }

    private int CalculateDiscountPercentage()
    {
        var hasSandwich = _items.Any(i => i.Type == ItemType.Sandwich);
        var hasFries = _items.Any(i => i.Type == ItemType.Fries);
        var hasDrink = _items.Any(i => i.Type == ItemType.Drink);

        if (hasSandwich && hasFries && hasDrink)
            return 20;

        if (hasSandwich && hasDrink)
            return 15;

        if (hasSandwich && hasFries)
            return 10;

        return 0;
    }
}
