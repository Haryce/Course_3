// [file name]: models/Employee.cs
namespace StoreG5G11.models;

public class Employee
{
    public int Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Salary { get; set; }
    
    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
}