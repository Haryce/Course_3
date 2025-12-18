using System;

namespace StoreG1G3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; } // Цена на момент заказа
        public int Quantity { get; set; }
        public decimal TotalAmount => Price * Quantity;

        // Навигационное свойство
        public virtual Product Product { get; set; }
    }
}