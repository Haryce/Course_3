// [file name]: views/EmployeesWindow.xaml.cs
using System.Windows;
using StoreG5G11.models;
using StoreG5G11.services;

namespace StoreG5G11.views;

public partial class EmployeesWindow : Window
{
    private DataService _dataService;
    
    public EmployeesWindow()
    {
        InitializeComponent();
        _dataService = DataService.Instance;
        LoadEmployees();
    }
    
    private void LoadEmployees()
    {
        employeesGrid.ItemsSource = null;
        employeesGrid.ItemsSource = _dataService.Employees;
    }
    
    private void AddEmployee_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new EmployeeEditWindow();
        if (dialog.ShowDialog() == true)
        {
            _dataService.AddEmployee(dialog.Employee);
            LoadEmployees();
        }
    }
    
    private void EditEmployee_Click(object sender, RoutedEventArgs e)
    {
        if (employeesGrid.SelectedItem is Employee selectedEmployee)
        {
            var dialog = new EmployeeEditWindow(selectedEmployee);
            if (dialog.ShowDialog() == true)
            {
                _dataService.UpdateEmployee(dialog.Employee);
                LoadEmployees();
            }
        }
        else
        {
            MessageBox.Show("Выберите сотрудника для редактирования", "Внимание", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
    private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
    {
        if (employeesGrid.SelectedItem is Employee selectedEmployee)
        {
            var result = MessageBox.Show($"Удалить сотрудника {selectedEmployee.FullName}?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                _dataService.DeleteEmployee(selectedEmployee.Id);
                LoadEmployees();
            }
        }
        else
        {
            MessageBox.Show("Выберите сотрудника для удаления", "Внимание", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}