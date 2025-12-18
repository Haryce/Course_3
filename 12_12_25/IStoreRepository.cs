using StoreG1G3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreG1G3.Data
{
    public interface IStoreRepository
    {
        // Product operations
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);

        // Employee operations
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);

        // Sale operations
        Task<IEnumerable<Sale>> GetSalesAsync();
        Task<Sale> GetSaleByIdAsync(int id);
        Task AddSaleAsync(Sale sale);
        Task<decimal> GetTotalSalesByDateAsync(DateTime date);

        // Reporting
        Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate);
    }

    public class SalesReport
    {
        public decimal TotalRevenue { get; set; }
        public int TotalSalesCount { get; set; }
        public Dictionary<string, int> TopProducts { get; set; }
        public DateTime ReportPeriodStart { get; set; }
        public DateTime ReportPeriodEnd { get; set; }
    }
}