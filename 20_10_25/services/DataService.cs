// [file name]: services/DataService.cs
using StoreG5G11.models;

namespace StoreG5G11.services;

public class DataService
{
    private static DataService? _instance;
    public static DataService Instance => _instance ??= new DataService();
    
    public List<Employee> Employees { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public List<Sale> Sales { get; set; } = new();
    private int _nextEmployeeId = 1;
    private int _nextProductId = 1;
    private int _nextSaleId = 1;
    private int _nextOrderId = 1;
    
    private DataService()
    {
        // Инициализация тестовыми данными
        InitializeTestData();
    }
    
    private void InitializeTestData()
    {
        // Тестовые сотрудники
        Employees.Add(new Employee { 
            Id = _nextEmployeeId++, 
            LastName = "Иванов", 
            FirstName = "Иван", 
            MiddleName = "Иванович", 
            BirthDate = new DateTime(1990, 5, 15), 
            Salary = 45000 
        });
        Employees.Add(new Employee { 
            Id = _nextEmployeeId++, 
            LastName = "Петрова", 
            FirstName = "Мария", 
            MiddleName = "Сергеевна", 
            BirthDate = new DateTime(1995, 8, 22), 
            Salary = 42000 
        });
        
        // Тестовые товары - кофе
        Products.Add(new Product { 
            Id = _nextProductId++, 
            Name = "Эспрессо", 
            Description = "Крепкий черный кофе", 
            Price = 150, 
            Category = "Coffee" 
        });
        Products.Add(new Product { 
            Id = _nextProductId++, 
            Name = "Капучино", 
            Description = "Кофе с молочной пенкой", 
            Price = 200, 
            Category = "Coffee" 
        });
        
        // Тестовые товары - выпечка
        Products.Add(new Product { 
            Id = _nextProductId++, 
            Name = "Круассан", 
            Description = "Слоеная выпечка", 
            Price = 120, 
            Category = "Bakery" 
        });
        Products.Add(new Product { 
            Id = _nextProductId++, 
            Name = "Шоколадный маффин", 
            Description = "Маффин с шоколадом", 
            Price = 180, 
            Category = "Bakery" 
        });
    }
    
    // Методы для работы с сотрудниками
    public void AddEmployee(Employee employee)
    {
        employee.Id = _nextEmployeeId++;
        Employees.Add(employee);
    }
    
    public void UpdateEmployee(Employee employee)
    {
        var existing = Employees.FirstOrDefault(e => e.Id == employee.Id);
        if (existing != null)
        {
            Employees.Remove(existing);
            Employees.Add(employee);
            Employees = Employees.OrderBy(e => e.Id).ToList();
        }
    }
    
    public void DeleteEmployee(int id)
    {
        var employee = Employees.FirstOrDefault(e => e.Id == id);
        if (employee != null)
        {
            Employees.Remove(employee);
        }
    }
    
    // Методы для работы с товарами
    public void AddProduct(Product product)
    {
        product.Id = _nextProductId++;
        Products.Add(product);
    }
    
    public void UpdateProduct(Product product)
    {
        var existing = Products.FirstOrDefault(p => p.Id == product.Id);
        if (existing != null)
        {
            Products.Remove(existing);
            Products.Add(product);
            Products = Products.OrderBy(p => p.Id).ToList();
        }
    }
    
    public void DeleteProduct(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            Products.Remove(product);
        }
    }
    
    // Методы для работы с продажами
    public int CreateOrder(List<OrderItem> items)
    {
        return _nextOrderId++;
    }
    
    public void AddSale(Sale sale)
    {
        sale.Id = _nextSaleId++;
        sale.SaleDate = DateTime.Now;
        Sales.Add(sale);
    }
    
    public List<Sale> GetSalesByDate(DateTime date)
    {
        return Sales.Where(s => s.SaleDate.Date == date.Date).ToList();
    }
}