﻿using API.Responses.Models.Relays;
using VPNClient.Classes;

namespace VPNClient.Pages;

public partial class ConnectPage : ContentPage
{
    private WgTunnelState TunnelState { get; set; } = WgTunnelState.Down;

    public ConnectPage()
    {
        InitializeComponent();

        CountryPicker.ItemsSource = SessionManager.Countries;

        CountryPicker.SelectedIndex = 0;
        CityPicker.SelectedIndex = 0;
        RelayPicker.SelectedIndex = 0;
    }

    private async void ConnectButton_Clicked(object sender, EventArgs e)
    {
        if(TunnelState != WgTunnelState.Down)
        {
            WireguardManager.DisconnectFromRelay();
            return;
        }

        var relay = (APIRelay)RelayPicker.SelectedItem;
        if (relay != null)
        {
            WireguardManager.SetTunnelStateChangeCallback((state) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TunnelState = state;
                    UpdateTunnelStateUI();
                });
            });
            await WireguardManager.ConnectToRelay(relay);
        }
    }

    private void CountryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var country = (APICountry)CountryPicker.SelectedItem;
        if (country != null)
        {
            CityPicker.ItemsSource = country.Cities;
            CityPicker.ItemsSource = CityPicker.GetItemsAsArray();
            CityPicker.ItemsSource = country.Cities;
        } 
        else
            CityPicker.ItemsSource = null;
    }

    private void CityPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var city = (APICity)CityPicker.SelectedItem;
        if (city != null)
        {
            RelayPicker.ItemsSource = city.Relays;
            RelayPicker.ItemsSource = RelayPicker.GetItemsAsArray();
            RelayPicker.ItemsSource = city.Relays;
        }
        else
            RelayPicker.ItemsSource = null;
    }

    private void UpdateTunnelStateUI()
    {
        StatusLabel.Text = TunnelState.ToString();
        ConnectButton.Text = TunnelState != WgTunnelState.Down ? "Disconnect" : "Connect";
    }
}