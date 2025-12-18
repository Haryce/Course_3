using System;

namespace StoreG1G3.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; } // Количество на складе
        public string Category { get; set; } // Категория фигурки
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public override string ToString()
        {
            return $"{Name} - {Price:C}";
        }
    }
}