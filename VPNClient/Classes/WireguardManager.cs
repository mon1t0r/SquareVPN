using API.Responses.Models.Relays;

namespace VPNClient.Classes
{
    public class WireguardManager
    {
        private static Action<string> OnTunnelStateChange;

        public static async Task ConnectToRelay(APIRelay relay)
        {
            await SessionManager.CurrentSession.ConnectPeer(relay.Hostname);

#if ANDROID
            MainActivity.Instance.ConnectToRelay(relay);
#elif WINDOWS
            //TODO: connect windows
#endif
        }

        public static void SetTunnelStateChangeCallback(Action<string> callback) =>
            OnTunnelStateChange = callback;

        public static void CallTunnelStateChange(string state) =>
            OnTunnelStateChange?.Invoke(state);
    }
}
