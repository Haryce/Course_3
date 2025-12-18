using System;

namespace StoreG1G3.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; } // Должность: Director, Accountant, Seller
        public DateTime HireDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;

        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}