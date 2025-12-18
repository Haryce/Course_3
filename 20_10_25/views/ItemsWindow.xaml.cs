// [file name]: views/ItemsWindow.xaml.cs
using System.Windows;
using StoreG5G11.models;
using StoreG5G11.services;

namespace StoreG5G11.views;

public partial class ItemsWindow : Window
{
    private DataService _dataService;
    
    public ItemsWindow()
    {
        InitializeComponent();
        _dataService = DataService.Instance;
        LoadProducts();
    }
    
    private void LoadProducts()
    {
        productsGrid.ItemsSource = null;
        productsGrid.ItemsSource = _dataService.Products;
    }
    
    private void AddProduct_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ProductEditWindow();
        if (dialog.ShowDialog() == true)
        {
            _dataService.AddProduct(dialog.Product);
            LoadProducts();
        }
    }
    
    private void EditProduct_Click(object sender, RoutedEventArgs e)
    {
        if (productsGrid.SelectedItem is Product selectedProduct)
        {
            var dialog = new ProductEditWindow(selectedProduct);
            if (dialog.ShowDialog() == true)
            {
                _dataService.UpdateProduct(dialog.Product);
                LoadProducts();
            }
        }
        else
        {
            MessageBox.Show("Выберите товар для редактирования", "Внимание", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
    private void DeleteProduct_Click(object sender, RoutedEventArgs e)
    {
        if (productsGrid.SelectedItem is Product selectedProduct)
        {
            var result = MessageBox.Show($"Удалить товар {selectedProduct.Name}?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                _dataService.DeleteProduct(selectedProduct.Id);
                LoadProducts();
            }
        }
        else
        {
            MessageBox.Show("Выберите товар для удаления", "Внимание", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    
    private void FilterByCategory_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (filterComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem selectedItem)
        {
            string? category = selectedItem.Tag as string;
            
            if (string.IsNullOrEmpty(category))
            {
                LoadProducts();
            }
            else
            {
                productsGrid.ItemsSource = _dataService.Products
                    .Where(p => p.Category == category)
                    .ToList();
            }
        }
    }
}