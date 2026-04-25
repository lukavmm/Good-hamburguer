using System;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class MenuItem
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public ItemType Type { get; private set; }

    public MenuItem(string name, decimal price, ItemType type)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Type = type;
    }

    public void Update(string name, decimal price, ItemType type)
    {
        Name = name;
        Price = price;
        Type = type;
    }

    private MenuItem() { }
}
