// [file name]: models/Sale.cs
namespace StoreG5G11.models;

public class Sale
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime SaleDate { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
}