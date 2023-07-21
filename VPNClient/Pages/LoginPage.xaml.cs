using API;
using VPNClient.Classes;

namespace VPNClient;

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

        bool result = await SessionManager.CurrentSession.Login(userId, WireguardKeyUtils.GenKeyPair(), (message) => { }, null);

        if (result)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}