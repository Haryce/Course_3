// [file name]: views/EmployeeEditWindow.xaml.cs
using System.Windows;
using StoreG5G11.models;

namespace StoreG5G11.views;

public partial class EmployeeEditWindow : Window
{
    public Employee Employee { get; private set; }
    
    public EmployeeEditWindow()
    {
        InitializeComponent();
        Employee = new Employee();
        DataContext = Employee;
    }
    
    public EmployeeEditWindow(Employee employee)
    {
        InitializeComponent();
        Employee = new Employee
        {
            Id = employee.Id,
            LastName = employee.LastName,
            FirstName = employee.FirstName,
            MiddleName = employee.MiddleName,
            BirthDate = employee.BirthDate,
            Salary = employee.Salary
        };
        DataContext = Employee;
        Title = "Редактирование сотрудника";
    }
    
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Employee.LastName) || 
            string.IsNullOrWhiteSpace(Employee.FirstName))
        {
            MessageBox.Show("Заполните обязательные поля: Фамилия и Имя", "Ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        DialogResult = true;
        Close();
    }
    
    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}