using VPNClient.Classes;
using VPNClient.Pages;

namespace VPNClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = SessionManager.CurrentSession.IsActive ? new AppShell() : new LoginPage();
        }
    }
}