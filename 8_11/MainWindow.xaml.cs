using System.Windows;
using System.Windows.Controls;

namespace AuthApp
{
    public partial class MainWindow : Window
    {
        public AuthViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new AuthViewModel();
            this.DataContext = ViewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordBox.Password;
            
            if (ViewModel.Login())
            {
                StatusText.Text = "✅ Успешный вход!";
                StatusText.Foreground = System.Windows.Media.Brushes.Green;
                
                // Здесь можно открыть главное окно приложения
                MessageBox.Show($"Добро пожаловать, {ViewModel.CurrentUser.Username}!", 
                              "Успешный вход", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
            else
            {
                StatusText.Text = "❌ Неверный логин или пароль";
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(ViewModel);
            registerWindow.Owner = this;
            registerWindow.ShowDialog();
        }
    }
}