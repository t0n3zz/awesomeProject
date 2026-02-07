using System.Configuration;
using System.Data;
using System.Windows;

namespace Pr14
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Pr14.DbService.Initialize();
        }

        public static Models.User CurrentUser { get; set; }
    }

}
