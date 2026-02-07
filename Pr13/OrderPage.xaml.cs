using Microsoft.Data.Sqlite;
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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        decimal totalSum = 0;

        public OrderPage()
        {
            InitializeComponent();
            FinalItemsList.ItemsSource = App.Basket;
            totalSum = App.Basket.Sum(p => p.Price);
            TxtTotal.Text = $"К оплате: {totalSum} руб.";
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (App.Basket.Count == 0)
            {
                System.Windows.MessageBox.Show("Корзина пуста!");
                return;
            }

            System.Windows.MessageBox.Show("Заказ успешно оформлен! Спасибо за покупку.");

            App.Basket.Clear();

            using (var connection = new SqliteConnection("Data Source=shop.db"))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Orders (FullName, Email, Address, TotalSum, OrderDate) VALUES (@name, @email, @addr, @sum, @date)";
                cmd.Parameters.AddWithValue("@name", TxtName.Text);
                cmd.Parameters.AddWithValue("@email", TxtEmail.Text);
                cmd.Parameters.AddWithValue("@addr", TxtAddress.Text);
                cmd.Parameters.AddWithValue("@sum", totalSum);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                cmd.ExecuteNonQuery();
            }

            App.Basket.Clear();
            NavigationService.Navigate(new MainPage());
        }
    }
}
