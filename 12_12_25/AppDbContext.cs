using Microsoft.EntityFrameworkCore;
using StoreG1G3.Models;

namespace StoreG1G3.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Используем SQL Server LocalDB или SQLite для простоты
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=StoreG1G3DB;Trusted_Connection=True;");
            // или для SQLite:
            // optionsBuilder.UseSqlite("Data Source=store.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Orders)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Индексы для оптимизации
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.LastName);

            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.SaleDate);

            // Начальные данные
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Пример начальных данных для товаров
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Фигурка Персонажа А", Description = "Коллекционная фигурка высшего качества", Price = 2999.99m, StockQuantity = 10, Category = "Аниме" },
                new Product { Id = 2, Name = "Фигурка Персонажа B", Description = "Эксклюзивная лимитированная серия", Price = 4999.99m, StockQuantity = 5, Category = "Кино" },
                new Product { Id = 3, Name = "Фигурка Персонажа C", Description = "Подвижные части, детализация", Price = 3499.99m, StockQuantity = 15, Category = "Игры" }
            );

            // Пример начальных данных для сотрудников
            modelBuilder.Entity<Employee>().HasData(
                new Employee 
                { 
                    Id = 1, 
                    FirstName = "Иван", 
                    LastName = "Иванов", 
                    MiddleName = "Иванович", 
                    BirthDate = new DateTime(1985, 5, 15),
                    Salary = 50000,
                    Position = "Director",
                    HireDate = new DateTime(2020, 1, 15)
                },
                new Employee 
                { 
                    Id = 2, 
                    FirstName = "Мария", 
                    LastName = "Петрова", 
                    MiddleName = "Сергеевна",
                    BirthDate = new DateTime(1990, 8, 22),
                    Salary = 40000,
                    Position = "Accountant",
                    HireDate = new DateTime(2021, 3, 10)
                }
            );
        }
    }
}