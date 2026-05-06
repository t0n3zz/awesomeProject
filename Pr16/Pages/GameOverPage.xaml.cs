using Pr16.Pages;
using System.Windows;
using System.Windows.Controls;

namespace Pr16.Pages
{
    public partial class GameOverPage : Page
    {
        public GameOverPage()
        {
            InitializeComponent();
            RestartButton.Click += Restart_Click;
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GamePage());
        }
    }
}