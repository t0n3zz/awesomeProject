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

namespace Pr13
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ProductsList.ItemsSource = DbService.GetProducts();
        }
        private void AddToBasket_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            var product = button.Tag as Pr13.Models.Product;

            if (product != null)
            {
                App.Basket.Add(product);
            }
        }

        private void GoToCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BasketPage());
        }
    }
}
