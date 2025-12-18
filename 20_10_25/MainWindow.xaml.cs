// [file name]: MainWindow.xaml.cs (обновленный)
using System.Text;
using System.Windows;
using StoreG5G11.views;

namespace StoreG5G11;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Console.OutputEncoding = Encoding.UTF8;
    }
    
    private void OpenEmployeesWindow_Click(object sender, RoutedEventArgs e)
    {
        var window = new EmployeesWindow();
        window.Show();
    }
    
    private void OpenProductsWindow_Click(object sender, RoutedEventArgs e)
    {
        var window = new ItemsWindow();
        window.Show();
    }
    
    private void OpenSalesWindow_Click(object sender, RoutedEventArgs e)
    {
        var window = new SalesWindow();
        window.Show();
    }
    
    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}