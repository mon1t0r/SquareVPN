using API;
using VPNClient.Classes;

namespace VPNClient.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        if (!ulong.TryParse(UserIdEntry.Text, out ulong userId))
            return;

        bool result = await SessionManager.CurrentSession.Login(userId, WireguardKeyUtils.GenKeyPair(),
            (devices) =>
            {
                Application.Current.MainPage = new NavigationPage(new LoginRemoveDevicePage(devices, async (device) =>
                {
                    result = await SessionManager.CurrentSession.Login(userId, WireguardKeyUtils.GenKeyPair(), (d) => { }, device.UUID.ToString());

                    if (result)
                        Application.Current.MainPage = new AppShell();
                }));
            }, null);

        if (result)
            Application.Current.MainPage = new AppShell();
    }
}