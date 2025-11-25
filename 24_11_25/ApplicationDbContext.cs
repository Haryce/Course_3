using Microsoft.EntityFrameworkCore;
using SneakerShop.Models;

namespace SneakerShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Sneaker> Sneakers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для Sneaker
            modelBuilder.Entity<Sneaker>()
                .HasIndex(s => new { s.Name, s.Size })
                .IsUnique();

            modelBuilder.Entity<Sneaker>()
                .Property(s => s.Price)
                .HasPrecision(10, 2);

            // Конфигурация для Order
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(10, 2);

            // Конфигурация для OrderItem
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(10, 2);

            // Заполнение начальными данными
            modelBuilder.Entity<Brand>().HasData(
                new Brand { BrandId = 1, Name = "Nike", Country = "USA", Description = "Just Do It" },
                new Brand { BrandId = 2, Name = "Adidas", Country = "Germany", Description = "Impossible is Nothing" },
                new Brand { BrandId = 3, Name = "New Balance", Country = "USA", Description = "Fearlessly Independent" },
                new Brand { BrandId = 4, Name = "Puma", Country = "Germany", Description = "Forever Faster" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Беговые", Description = "Кроссовки для бега" },
                new Category { CategoryId = 2, Name = "Баскетбольные", Description = "Кроссовки для баскетбола" },
                new Category { CategoryId = 3, Name = "Повседневные", Description = "Повседневные кроссовки" },
                new Category { CategoryId = 4, Name = "Лимитированные", Description = "Лимитированные серии" }
            );

            modelBuilder.Entity<Sneaker>().HasData(
                new Sneaker 
                { 
                    SneakerId = 1, 
                    Name = "Nike Air Max 270", 
                    Description = "Комфортные кроссовки с технологией Air Max",
                    Price = 12999.99m, 
                    BrandId = 1, 
                    CategoryId = 3, 
                    Size = 42, 
                    Color = "Black/White", 
                    StockQuantity = 15,
                    ImageUrl = "nike_air_max_270.jpg"
                },
                new Sneaker 
                { 
                    SneakerId = 2, 
                    Name = "Adidas Ultraboost 22", 
                    Description = "Беговые кроссовки с технологией Boost",
                    Price = 14999.99m, 
                    BrandId = 2, 
                    CategoryId = 1, 
                    Size = 43, 
                    Color = "Blue", 
                    StockQuantity = 10,
                    ImageUrl = "adidas_ultraboost.jpg"
                }
            );
        }
    }
}