
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Pr16.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GamePage());
        }
    }
}