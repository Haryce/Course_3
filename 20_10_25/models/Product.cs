// [file name]: models/Product.cs
namespace StoreG5G11.models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty; // "Coffee" или "Bakery"
}