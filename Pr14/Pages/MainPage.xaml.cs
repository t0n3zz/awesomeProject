using Pr14.Models;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            FilmsList.ItemsSource = DbService.GetFilms();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var allFilms = DbService.GetFilms();
            FilmsList.ItemsSource = allFilms.Where(f => f.Title.ToLower().Contains(TxtSearch.Text.ToLower())).ToList();
        }

        private void SortCombo_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var currentList = (List<Film>)FilmsList.ItemsSource;
            if (SortCombo.SelectedIndex == 0)
                FilmsList.ItemsSource = currentList.OrderBy(f => f.Title).ToList();
            else
                FilmsList.ItemsSource = currentList.OrderByDescending(f => f.Rating).ToList();
        }

        private void GoToProfile_Click(object sender, RoutedEventArgs ee) 
        {
            NavigationService.Navigate(new SignInPage());
        }

        private void SelectFilm_Click(object sender, RoutedEventArgs ee)
        {
            var selectedFilm = (sender as Button).Tag as Pr14.Models.Film;

            if (selectedFilm != null)
            {
                NavigationService.Navigate(new SessionPage(selectedFilm));
            }
        }
    }
}
