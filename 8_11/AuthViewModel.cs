using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AuthApp
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private User _currentUser = new User();
        private User _newUser = new User();
        private string _confirmPassword = "";
        private bool _rememberMe;
        private bool _acceptTerms;
        private string _usernameValidation = "";
        private string _emailValidation = "";
        private string _passwordValidation = "";
        private string _confirmPasswordValidation = "";

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public User NewUser
        {
            get => _newUser;
            set
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                _rememberMe = value;
                OnPropertyChanged();
            }
        }

        public bool AcceptTerms
        {
            get => _acceptTerms;
            set
            {
                _acceptTerms = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        // Валидационные сообщения
        public string UsernameValidation
        {
            get => _usernameValidation;
            set
            {
                _usernameValidation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string EmailValidation
        {
            get => _emailValidation;
            set
            {
                _emailValidation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string PasswordValidation
        {
            get => _passwordValidation;
            set
            {
                _passwordValidation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string ConfirmPasswordValidation
        {
            get => _confirmPasswordValidation;
            set
            {
                _confirmPasswordValidation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public bool CanRegister => 
            string.IsNullOrEmpty(UsernameValidation) &&
            string.IsNullOrEmpty(EmailValidation) &&
            string.IsNullOrEmpty(PasswordValidation) &&
            string.IsNullOrEmpty(ConfirmPasswordValidation) &&
            AcceptTerms &&
            !string.IsNullOrWhiteSpace(NewUser.Username) &&
            !string.IsNullOrWhiteSpace(NewUser.Email) &&
            !string.IsNullOrWhiteSpace(NewUser.Password);

        public string Password { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        public void StartNewRegistration()
        {
            NewUser = new User();
            ConfirmPassword = "";
            AcceptTerms = false;
            ClearValidations();
        }

        public bool Login()
        {
            // В реальном приложении здесь была бы проверка в базе данных
            return !string.IsNullOrWhiteSpace(CurrentUser.Username) && 
                   !string.IsNullOrWhiteSpace(Password);
        }

        public bool Register()
        {
            ValidateAll();
            
            if (!CanRegister) return false;

            // В реальном приложении здесь была бы запись в базу данных
            CurrentUser.Username = NewUser.Username;
            Password = NewUser.Password;

            return true;
        }

        public void ValidateAll()
        {
            ValidateUsername();
            ValidateEmail();
            ValidatePasswords();
        }

        private void ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(NewUser.Username))
            {
                UsernameValidation = "Логин обязателен";
            }
            else if (NewUser.Username.Length < 3)
            {
                UsernameValidation = "Логин должен быть не менее 3 символов";
            }
            else
            {
                UsernameValidation = "";
            }
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(NewUser.Email))
            {
                EmailValidation = "Email обязателен";
            }
            else if (!Regex.IsMatch(NewUser.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                EmailValidation = "Некорректный формат email";
            }
            else
            {
                EmailValidation = "";
            }
        }

        public void ValidatePasswords()
        {
            if (string.IsNullOrWhiteSpace(NewUser.Password))
            {
                PasswordValidation = "Пароль обязателен";
            }
            else if (NewUser.Password.Length < 6)
            {
                PasswordValidation = "Пароль должен быть не менее 6 символов";
            }
            else
            {
                PasswordValidation = "";
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmPasswordValidation = "Подтверждение пароля обязательно";
            }
            else if (NewUser.Password != ConfirmPassword)
            {
                ConfirmPasswordValidation = "Пароли не совпадают";
            }
            else
            {
                ConfirmPasswordValidation = "";
            }
        }

        private void ClearValidations()
        {
            UsernameValidation = "";
            EmailValidation = "";
            PasswordValidation = "";
            ConfirmPasswordValidation = "";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}