using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using WinUIEx;
#elif ANDROID
using VPNClient.Platforms.Android.UI;
#endif

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
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
                    handlers.AddHandler<CustomTextCell, CustomTextCellHandler>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        window.CenterOnScreen(450, 780);
                    });
                });
            });
#endif

            return builder.Build();
        }
    }
}