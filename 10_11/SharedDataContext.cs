using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TwoWindowsApp
{
    public class SharedDataContext : INotifyPropertyChanged
    {
        private string _sharedText = "";

        public string SharedText
        {
            get => _sharedText;
            set
            {
                _sharedText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}