using Android.App;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using API.Responses.Models.Relays;
using Com.Wireguard.Android.Backend;
using Com.Wireguard.Config;
using VPNClient.Classes;
using VPNClient.Platforms.Android.Classes;

namespace VPNClient
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Instance = this;
        }

        public void ConnectToRelay(APIRelay relay)
        {
            var tunnel = new WgTunnel();
            var intentPrepare = VpnService.Prepare(this);
            if (intentPrepare != null)
                StartActivityForResult(intentPrepare, 0);

            var interfaceBuilder = new Interface.Builder();
            var peerBuilder = new Peer.Builder();

            var backend = new GoBackend(this);

            Task.Run(() =>
            {
                backend.SetState(tunnel, ITunnel.State.Up, new Config.Builder()
                            .SetInterface(interfaceBuilder.AddAddress(InetNetwork.Parse($"{SessionManager.CurrentSession.Device.IPV4Address}/32")).ParsePrivateKey(SessionManager.CurrentSession.PrivateKey).Build())
                            .AddPeer(peerBuilder.AddAllowedIp(InetNetwork.Parse("0.0.0.0/0")).SetEndpoint(InetEndpoint.Parse($"{relay.IPV4}:{relay.Port}")).ParsePublicKey(relay.PublicKey).Build())
                            .Build());
            });
        }
    }
}