using CommunityToolkit.Maui;
using VPNClient.Classes;

namespace VPNClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var app = builder.Build();

            Task.Run(async () =>
            {
                SessionManager.Initialize();
                //await SessionManager.LoadSessionAsync();
            }).Wait();

            return app;
        }
    }
}