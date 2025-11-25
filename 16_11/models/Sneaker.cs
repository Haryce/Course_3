using System;

namespace SneakerShop.Models
{
    public class Sneaker
    {
        public int SneakerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

// Models/Brand.cs
namespace SneakerShop.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
    }
}

// Models/Category.cs
namespace SneakerShop.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

// Models/Customer.cs
using System;

namespace SneakerShop.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}

// ViewModels/SneakerViewModel.cs
using System;

namespace SneakerShop.ViewModels
{
    public class SneakerViewModel
    {
        public int SneakerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}