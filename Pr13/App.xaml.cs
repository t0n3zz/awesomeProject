using System.Configuration;
using System.Data;
using System.Windows;

namespace Pr13
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static List<Pr13.Models.Product> Basket = new List<Pr13.Models.Product>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Pr13.DbService.Initialize();
        }
    }
}
