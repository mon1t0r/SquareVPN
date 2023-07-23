using VPNClient.Classes;

namespace VPNClient.Pages;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
		DeviceNameEntry.Text = SessionManager.CurrentSession.Device.Name;
		if (SessionManager.PaidUntil != null)
		{
			PaidUntilEntry.Text = SessionManager.PaidUntil.ToString();
			if (SessionManager.PaidUntil.Value < DateTime.Now)
				PaidUntilEntry.TextColor = Colors.Red;
        }
		
	}

    private async void LogoutButton_Clicked(object sender, EventArgs e) =>
        await SessionManager.CurrentSession.Logout();
}