using StoreG1G3.Data;
using StoreG1G3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreG1G3.Services
{
    public class StoreService
    {
        private readonly AppDbContext _context;

        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        // Продажа товара
        public async Task<Sale> ProcessSaleAsync(int employeeId, Dictionary<int, int> productsWithQuantities)
        {
            var sale = new Sale
            {
                EmployeeId = employeeId,
                SaleDate = DateTime.Now,
                ReceiptNumber = GenerateReceiptNumber(),
                PaymentMethod = "Cash" // или "Card"
            };

            foreach (var item in productsWithQuantities)
            {
                var product = await _context.Products.FindAsync(item.Key);
                if (product == null || product.StockQuantity < item.Value)
                    throw new InvalidOperationException($"Товар {item.Key} недоступен");

                var order = new Order
                {
                    ProductId = product.Id,
                    Price = product.Price,
                    Quantity = item.Value
                };

                sale.AddOrder(order);
                product.StockQuantity -= item.Value;
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return sale;
        }

        private string GenerateReceiptNumber()
        {
            return $"RCPT-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

        // Получение статистики
        public async Task<SalesStatistics> GetSalesStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var sales = _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToList();

            return new SalesStatistics
            {
                Period = $"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}",
                TotalSales = sales.Count,
                TotalRevenue = sales.Sum(s => s.TotalAmount),
                AverageSale = sales.Any() ? sales.Average(s => s.TotalAmount) : 0,
                TopEmployee = sales.GroupBy(s => s.EmployeeId)
                    .Select(g => new { EmployeeId = g.Key, Total = g.Sum(s => s.TotalAmount) })
                    .OrderByDescending(x => x.Total)
                    .FirstOrDefault()?.EmployeeId
            };
        }
    }

    public class SalesStatistics
    {
        public string Period { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageSale { get; set; }
        public int? TopEmployee { get; set; }
    }
}