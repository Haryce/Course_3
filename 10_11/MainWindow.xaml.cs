using System.Windows;

namespace TwoWindowsApp
{
    public partial class MainWindow : Window
    {
        public SharedDataContext DataContext { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SharedDataContext();
            this.DataContext = DataContext;
        }

        private void OpenSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            var secondWindow = new SecondWindow(DataContext);
            secondWindow.Show();
        }
    }
}