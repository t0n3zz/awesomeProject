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
    /// Логика взаимодействия для HallPage.xaml
    /// </summary>
    public partial class HallPage : Page
    {
        Session _session;
        List<int> _occupiedSeats;

        public HallPage(Session session)
        {
            InitializeComponent();
            _session = session;
            TxtSessionInfo.Text = $"{session.HallName} | {session.SessionTime}";

            LoadHall();
        }

        public class Seat
        {
            public int Number { get; set; }
            public bool IsAvailable { get; set; }
            public string Color => IsAvailable ? "LightGreen" : "LightGray";
        }

        private void LoadHall()
        {
            _occupiedSeats = DbService.GetOccupiedSeats(_session.Id);

            var seats = new List<Seat>();
            for (int i = 1; i <= 25; i++)
            {
                seats.Add(new Seat
                {
                    Number = i,
                    IsAvailable = !_occupiedSeats.Contains(i)
                });
            }
            LbxSeats.ItemsSource = seats;
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            var seat = (sender as Button).Tag as Seat;

            if (App.CurrentUser == null)
            {
                MessageBox.Show("Сначала войдите в аккаунт!");
                return;
            }

            var res = MessageBox.Show($"Забронировать место №{seat.Number}?", "Подтверждение", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                DbService.BuyTicket(App.CurrentUser.Id, _session.Id, seat.Number, 350);
                MessageBox.Show("Билет куплен!");
                LoadHall();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
