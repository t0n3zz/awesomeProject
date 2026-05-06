using Pr16.Pages;
using System.Windows;


namespace Pr16
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new StartPage());
        }
    }
}
