using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SneakerShop.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public virtual ICollection<Order> Orders { get; set; }
    }
}