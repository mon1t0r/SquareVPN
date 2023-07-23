using VPNClient.Classes;

namespace VPNClient.Pages;

public partial class NoInternetConnectionPage : ContentPage
{
	public NoInternetConnectionPage()
	{
		InitializeComponent();
	}

    private async void RetryButton_Clicked(object sender, EventArgs e)
    {
        bool noConnection = false;

        try
        {
            await SessionManager.Initialize();
        }
        catch
        {
            noConnection = true;
        }

        if (!noConnection && Application.Current is App)
            (Application.Current as App).SelectPage();
    }
}