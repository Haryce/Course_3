using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SneakerShop.Models
{
    public class Sneaker
    {
        [Key]
        public int SneakerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [Range(0, 50)]
        public int Size { get; set; }

        [Required]
        [StringLength(20)]
        public string Color { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}