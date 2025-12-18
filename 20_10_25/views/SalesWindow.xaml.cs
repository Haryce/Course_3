// [file name]: views/SalesWindow.xaml.cs
using System.Windows;
using System.Windows.Controls;
using StoreG5G11.models;
using StoreG5G11.services;

namespace StoreG5G11.views;

public partial class SalesWindow : Window
{
    private DataService _dataService;
    private List<OrderItem> _currentOrder = new();
    
    public SalesWindow()
    {
        InitializeComponent();
        _dataService = DataService.Instance;
        InitializeData();
    }
    
    private void InitializeData()
    {
        // Загрузка сотрудников
        employeeComboBox.ItemsSource = _dataService.Employees;
        employeeComboBox.DisplayMemberPath = "FullName";
        employeeComboBox.SelectedValuePath = "Id";
        
        // Загрузка товаров
        productsComboBox.ItemsSource = _dataService.Products;
        productsComboBox.DisplayMemberPath = "Name";
        productsComboBox.SelectedValuePath = "Id";
        
        // Загрузка продаж за сегодня
        LoadSalesForToday();
    }
    
    private void LoadSalesForToday()
    {
        var todaySales = _dataService.GetSalesByDate(DateTime.Today);
        salesGrid.ItemsSource = null;
        salesGrid.ItemsSource = todaySales;
        
        // Подсчет общей выручки за день
        decimal dailyTotal = todaySales.Sum(s => s.Total);
        dailyTotalTextBlock.Text = $"Выручка за день: {dailyTotal:N2} руб.";
    }
    
    private void AddToOrder_Click(object sender, RoutedEventArgs e)
    {
        if (productsComboBox.SelectedItem is Product selectedProduct && 
            int.TryParse(quantityTextBox.Text, out int quantity) && quantity > 0)
        {
            var orderItem = new OrderItem
            {
                ProductId = selectedProduct.Id,
                ProductName = selectedProduct.Name,
                Price = selectedProduct.Price,
                Quantity = quantity
            };
            
            _currentOrder.Add(orderItem);
            UpdateOrderDisplay();
            
            // Сброс выбора
            productsComboBox.SelectedIndex = -1;
            quantityTextBox.Text = "1";
        }
        else
        {
            MessageBox.Show("Выберите товар и укажите количество", "Ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void UpdateOrderDisplay()
    {
        orderItemsGrid.ItemsSource = null;
        orderItemsGrid.ItemsSource = _currentOrder;
        
        decimal orderTotal = _currentOrder.Sum(item => item.Total);
        orderTotalTextBlock.Text = $"Итого: {orderTotal:N2} руб.";
    }
    
    private void RemoveFromOrder_Click(object sender, RoutedEventArgs e)
    {
        if (orderItemsGrid.SelectedItem is OrderItem selectedItem)
        {
            _currentOrder.Remove(selectedItem);
            UpdateOrderDisplay();
        }
    }
    
    private void CompleteSale_Click(object sender, RoutedEventArgs e)
    {
        if (employeeComboBox.SelectedItem is Employee selectedEmployee && _currentOrder.Any())
        {
            // Создание заказа
            int orderId = _dataService.CreateOrder(_currentOrder);
            
            // Создание продажи
            var sale = new Sale
            {
                OrderId = orderId,
                EmployeeId = selectedEmployee.Id,
                EmployeeName = selectedEmployee.FullName,
                Total = _currentOrder.Sum(item => item.Total),
                OrderItems = new List<OrderItem>(_currentOrder)
            };
            
            _dataService.AddSale(sale);
            
            // Очистка текущего заказа
            _currentOrder.Clear();
            UpdateOrderDisplay();
            
            // Обновление списка продаж
            LoadSalesForToday();
            
            MessageBox.Show("Продажа успешно оформлена!", "Успех", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Выберите сотрудника и добавьте товары в заказ", "Ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void ShowAllSales_Click(object sender, RoutedEventArgs e)
    {
        var allSalesWindow = new AllSalesWindow();
        allSalesWindow.ShowDialog();
    }
    
    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}