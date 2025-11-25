using System.Windows;
using System.Windows.Controls;

namespace AuthApp
{
    public partial class RegisterWindow : Window
    {
        public AuthViewModel ViewModel { get; }

        public RegisterWindow(AuthViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
            ViewModel.StartNewRegistration();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.NewUser.Password = PasswordBox.Password;
            ViewModel.ValidatePasswords();
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ConfirmPassword = ConfirmPasswordBox.Password;
            ViewModel.ValidatePasswords();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Register())
            {
                RegisterStatusText.Text = "✅ Регистрация успешна!";
                RegisterStatusText.Foreground = System.Windows.Media.Brushes.Green;
                
                MessageBox.Show("Регистрация завершена успешно!\nТеперь вы можете войти в систему.", 
                              "Успешная регистрация", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
                
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                RegisterStatusText.Text = "❌ Исправьте ошибки в форме";
                RegisterStatusText.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}