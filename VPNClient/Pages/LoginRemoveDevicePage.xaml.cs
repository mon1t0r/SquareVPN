using API.Responses.Models;

namespace VPNClient.Pages;

public partial class LoginRemoveDevicePage : ContentPage
{
	private readonly Action<APIDevice> ReplaceDeviceCallback;

    public LoginRemoveDevicePage(List<APIDevice> devices, Action<APIDevice> replaceDeviceCallback)
	{
		InitializeComponent();

		devices.ForEach((device) => device.CreatedUTC = device.CreatedUTC?.ToLocalTime());

		DevicesListView.ItemsSource = devices;
		ReplaceDeviceCallback = replaceDeviceCallback;
	}

    private void ReplaceDeviceButton_Clicked(object sender, EventArgs e)
    {
		if (DevicesListView.SelectedItem == null)
			return;
		ReplaceDeviceCallback.Invoke((APIDevice)DevicesListView.SelectedItem);
    }

    private void CancelButton_Clicked(object sender, EventArgs e) =>
        Application.Current.MainPage = new LoginPage();
}