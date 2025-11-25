// Program.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SneakerShop.Data;
using SneakerShop.Forms;
using SneakerShop.Repositories;
using SneakerShop.Services;
using System;
using System.Windows.Forms;

namespace SneakerShop
{
    internal static class Program
    {
        private static ServiceProvider ServiceProvider { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Настройка Dependency Injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Создание и применение миграций
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server=localhost;Database=SneakerShopDb;Trusted_Connection=true;TrustServerCertificate=true;"));

            // Repositories
            services.AddScoped<ISneakerRepository, SneakerRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Services
            services.AddScoped<ISneakerService, SneakerService>();

            // Forms
            services.AddScoped<MainForm>();
        }
    }
}