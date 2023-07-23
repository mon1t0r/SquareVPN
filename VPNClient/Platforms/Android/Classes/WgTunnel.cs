using Com.Wireguard.Android.Backend;
using VPNClient.Classes;

namespace VPNClient.Platforms.Android.Classes
{
    public class WgTunnel : Java.Lang.Object, ITunnel
    {
        public string Name => "wgpreconf";

        public void OnStateChange(ITunnel.State newState)
        {
            WgTunnelState state;
            if (newState == ITunnel.State.Up)
                state = WgTunnelState.Up;
            else if (newState == ITunnel.State.Toggle)
                state = WgTunnelState.Toggle;
            else
                state = WgTunnelState.Down;

            WireguardManager.CallTunnelStateChange(state);
        }
    }
}
