using API.Responses.Models.Relays;

namespace VPNClient.Classes
{
    public class WireguardManager
    {
        private static Action<WgTunnelState> OnTunnelStateChange;

        public static async Task ConnectToRelay(APIRelay relay)
        {
            if (!await SessionManager.CurrentSession.ConnectPeer(relay.Hostname))
                return;

#if ANDROID
            MainActivity.Instance.ConnectToRelay(relay);
#elif WINDOWS
            //TODO: connect windows
#endif
        }

        public static void DisconnectFromRelay()
        {
#if ANDROID
            MainActivity.DisconnectFromRelay();
#elif WINDOWS
            //TODO: connect windows
#endif
        }

        public static void SetTunnelStateChangeCallback(Action<WgTunnelState> callback) =>
            OnTunnelStateChange = callback;

        public static void CallTunnelStateChange(WgTunnelState state) =>
            OnTunnelStateChange?.Invoke(state);
    }
}
