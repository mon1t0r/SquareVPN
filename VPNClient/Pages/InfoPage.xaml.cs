using VPNClient.Classes;

namespace VPNClient.Pages;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
		DeviceNameEntry.Text = SessionManager.CurrentSession.Device.Name;
		PaidUntilEntry.Text = SessionManager.PaidUntil != null ? SessionManager.PaidUntil.ToString() : string.Empty;
	}

    private async void LogoutButton_Clicked(object sender, EventArgs e) =>
        await SessionManager.CurrentSession.Logout();
}