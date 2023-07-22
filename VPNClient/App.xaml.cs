using VPNClient.Classes;
using VPNClient.Pages;

namespace VPNClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = SessionManager.CurrentSession.IsActive || true ? new AppShell() : new NavigationPage(new LoginPage());
        }
    }
}