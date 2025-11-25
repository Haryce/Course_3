using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SneakerShop.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Country { get; set; }

        public virtual ICollection<Sneaker> Sneakers { get; set; }
    }
}