using Com.Wireguard.Android.Backend;
using VPNClient.Classes;

namespace VPNClient.Platforms.Android.Classes
{
    public class WgTunnel : Java.Lang.Object, ITunnel
    {
        public string Name => "wgpreconf";

        public void OnStateChange(ITunnel.State newState) =>
            WireguardManager.CallTunnelStateChange(newState.ToString());
    }
}
