using Android.App;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using API.Responses.Models.Relays;
using Com.Wireguard.Android.Backend;
using Com.Wireguard.Config;
using Java.Lang;
using Java.Net;
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

            PersistentConnectionProperties.Instance.Backend = new GoBackend(this);
        }

        public void ConnectToRelay(APIRelay relay)
        {
            var intentPrepare = VpnService.Prepare(this);
            if (intentPrepare != null)
                StartActivityForResult(intentPrepare, 0);

            var interfaceBuilder = new Interface.Builder();
            var peerBuilder = new Peer.Builder();

            PersistentConnectionProperties.Instance.Backend = new GoBackend(this);

            Task.Run(() =>
            {
                PersistentConnectionProperties.Instance.Backend
                .SetState(PersistentConnectionProperties.Instance.Tunnel, ITunnel.State.Up, new Config.Builder()
                            .SetInterface(interfaceBuilder
                                .AddAddress(InetNetwork.Parse($"{SessionManager.CurrentSession.Device.IPV4Address}/32"))
                                .AddDnsServers(new List<InetAddress>() { InetAddress.GetByName("8.8.8.8"), InetAddress.GetByName("1.1.1.1") })
                                .ParsePrivateKey(SessionManager.CurrentSession.PrivateKey).Build())
                            .AddPeer(peerBuilder
                                .AddAllowedIp(InetNetwork.Parse("0.0.0.0/0"))
                                .SetEndpoint(InetEndpoint.Parse($"{relay.IPV4}:{relay.Port}"))
                                .ParsePublicKey(relay.PublicKey).Build())
                            .Build());
            });
        }

        public static void DisconnectFromRelay()
        {
            PersistentConnectionProperties.Instance.Backend
                .SetState(PersistentConnectionProperties.Instance.Tunnel, ITunnel.State.Down, null);
        }
    }
}