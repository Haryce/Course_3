using System.Windows;

namespace TwoWindowsApp
{
    public partial class SecondWindow : Window
    {
        public SecondWindow(SharedDataContext dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext;
        }
    }
}