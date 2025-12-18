using System;

namespace StoreG1G3.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public string ReceiptNumber { get; set; } // Номер чека
        public string PaymentMethod { get; set; } // Способ оплаты

        // Навигационные свойства
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Метод для добавления товара в продажу
        public void AddOrder(Order order)
        {
            Orders.Add(order);
            TotalAmount += order.TotalAmount;
        }
    }
}