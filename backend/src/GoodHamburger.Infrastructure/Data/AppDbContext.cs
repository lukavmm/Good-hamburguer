using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            
            entity.Property(o => o.CreatedAt)
                .IsRequired();

            entity.Property(o => o.Subtotal)
                .HasPrecision(18, 2);

            entity.Property(o => o.DiscountAmount)
                .HasPrecision(18, 2);

            entity.Property(o => o.Total)
                .HasPrecision(18, 2);

            entity.HasMany<MenuItem>(o => o.Items)
                .WithMany()
                .UsingEntity("OrderItems");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(m => m.Id);
            
            entity.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(m => m.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(m => m.Type)
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Role)
                .IsRequired();

            entity.Property(u => u.CreatedAt)
                .IsRequired();
        });
    }
}
