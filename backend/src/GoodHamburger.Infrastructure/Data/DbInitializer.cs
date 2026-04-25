using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("admin");
            var adminUser = new User("admin", adminPasswordHash, UserRole.Admin);
            context.Users.Add(adminUser);
            context.SaveChanges();
        }

        if (context.MenuItems.Any())
            return;

        var menuItems = new[]
        {
            new MenuItem("X Burger", 5.00m, ItemType.Sandwich),
            new MenuItem("X Egg", 4.50m, ItemType.Sandwich),
            new MenuItem("X Bacon", 7.00m, ItemType.Sandwich),
            new MenuItem("Fries", 2.00m, ItemType.Fries),
            new MenuItem("Soft drink", 2.50m, ItemType.Drink)
        };

        context.MenuItems.AddRange(menuItems);
        context.SaveChanges();
    }
}
