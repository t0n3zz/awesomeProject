using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pr14.Pages
{
    /// <summary>
    /// Логика взаимодействия для SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {
        private Pr14.Models.Film _film;

        public SessionPage(Pr14.Models.Film film)
        {
            InitializeComponent();
            _film = film;
            this.DataContext = _film;

            LoadSessions();
        }

        private void LoadSessions()
        {
            SessionsList.ItemsSource = DbService.GetSessionsForFilm(_film.Id);
        }

        private void SelectSeat_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null) { MessageBox.Show("Войдите в аккаунт!"); return; }
            var session = (sender as Button).Tag as Pr14.Models.Session;

            if (session != null)
            {
                NavigationService.Navigate(new HallPage(session));
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
