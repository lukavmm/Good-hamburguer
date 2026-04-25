using FluentAssertions;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using Xunit;

namespace GoodHamburger.Domain.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void Order_ShouldCalculateSubtotal_WhenItemsAreAdded()
    {
        var order = new Order();
        var sandwich = new MenuItem("X Burger", 5.00m, ItemType.Sandwich);
        var fries = new MenuItem("Fries", 2.00m, ItemType.Fries);

        order.AddItem(sandwich);
        order.AddItem(fries);

        order.Subtotal.Should().Be(7.00m);
    }

    [Fact]
    public void Order_ShouldThrowException_WhenAddingDuplicateItem()
    {
        var order = new Order();
        var sandwich = new MenuItem("X Burger", 5.00m, ItemType.Sandwich);

        order.AddItem(sandwich);
        var act = () => order.AddItem(sandwich);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*duplicad*");
    }

    [Fact]
    public void Order_ShouldThrowException_WhenAddingMultipleSandwiches()
    {
        var order = new Order();
        var sandwich1 = new MenuItem("X Burger", 5.00m, ItemType.Sandwich);
        var sandwich2 = new MenuItem("X Egg", 4.50m, ItemType.Sandwich);

        order.AddItem(sandwich1);
        var act = () => order.AddItem(sandwich2);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*apenas um sanduíche*");
    }

    [Fact]
    public void Order_ShouldApply20PercentDiscount_WhenHasSandwichFriesAndDrink()
    {
        var order = new Order();
        order.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));
        order.AddItem(new MenuItem("Fries", 2.00m, ItemType.Fries));
        order.AddItem(new MenuItem("Soft drink", 2.50m, ItemType.Drink));

        order.CalculateTotal();

        order.Subtotal.Should().Be(9.50m);
        order.DiscountPercentage.Should().Be(20);
        order.DiscountAmount.Should().Be(1.90m);
        order.Total.Should().Be(7.60m);
    }

    [Fact]
    public void Order_ShouldApply15PercentDiscount_WhenHasSandwichAndDrink()
    {
        var order = new Order();
        order.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));
        order.AddItem(new MenuItem("Soft drink", 2.50m, ItemType.Drink));

        order.CalculateTotal();

        order.Subtotal.Should().Be(7.50m);
        order.DiscountPercentage.Should().Be(15);
        order.DiscountAmount.Should().Be(1.125m);
        order.Total.Should().Be(6.375m);
    }

    [Fact]
    public void Order_ShouldApply10PercentDiscount_WhenHasSandwichAndFries()
    {
        var order = new Order();
        order.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));
        order.AddItem(new MenuItem("Fries", 2.00m, ItemType.Fries));

        order.CalculateTotal();

        order.Subtotal.Should().Be(7.00m);
        order.DiscountPercentage.Should().Be(10);
        order.DiscountAmount.Should().Be(0.70m);
        order.Total.Should().Be(6.30m);
    }

    [Fact]
    public void Order_ShouldApplyNoDiscount_WhenHasOnlySandwich()
    {
        var order = new Order();
        order.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));

        order.CalculateTotal();

        order.Subtotal.Should().Be(5.00m);
        order.DiscountPercentage.Should().Be(0);
        order.DiscountAmount.Should().Be(0m);
        order.Total.Should().Be(5.00m);
    }

    [Fact]
    public void Order_ShouldBeInvalid_WhenNoSandwich()
    {
        var order = new Order();
        order.AddItem(new MenuItem("Fries", 2.00m, ItemType.Fries));

        var result = order.IsValid();

        result.Should().BeFalse();
    }

    [Fact]
    public void Order_ShouldBeValid_WhenHasSandwich()
    {
        var order = new Order();
        order.AddItem(new MenuItem("X Burger", 5.00m, ItemType.Sandwich));

        var result = order.IsValid();

        result.Should().BeTrue();
    }
}
