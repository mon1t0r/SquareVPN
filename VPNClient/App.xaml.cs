using VPNClient.Classes;
using VPNClient.Classes.Utils;
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
            SessionManager.Initialize();

            bool noConnection = false;

            Task.Run(async () =>
            {
                try
                {
                    await SessionManager.InitializeSession();
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

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
#if WINDOWS
            window.Title = Constants.AppName;
#endif
            return window;
        }
    }
}