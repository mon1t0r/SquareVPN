using VPNClient.Classes;
using VPNClient.Pages;

namespace VPNClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            bool noConnection = false;

            Task.Run(async () =>
            {
                try
                {
                    await SessionManager.Initialize();
                }
                catch
                {
                    noConnection = true;
                }
            }).Wait();

            if (noConnection)
                MainPage = new NoInternetConnectionPage();
            else
                SelectPage();
        }

        public void SelectPage()
        {
            if (SessionManager.CurrentSession.IsActive)
                MainPage = new AppShell();
            else
                MainPage = new LoginPage();
        }
    }
}