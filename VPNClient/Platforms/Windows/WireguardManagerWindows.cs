using API.Responses.Models.Relays;
using System.Text;
using Tunnel;
using VPNClient.Classes;

namespace VPNClient
{
    public class WireguardManagerWindows
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wireguard");
        private static readonly string ErrorFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error");

        public static async Task ConnectToRelay(APIRelay relay)
        {
            try { File.Delete(ConfigFilePath); } catch { }

            var configString = $"[Interface]\nPrivateKey = {SessionManager.CurrentSession.PrivateKey}\nAddress = {SessionManager.CurrentSession.Device.IPV4Address}/32\nDNS = 8.8.8.8, 1.1.1.1\n\n[Peer]\nPublicKey = {relay.PublicKey}\nEndpoint = {relay.IPV4}:{relay.Port}\nAllowedIPs = 0.0.0.0/0";
            await File.WriteAllBytesAsync(ConfigFilePath, Encoding.UTF8.GetBytes(configString));

            await Task.Run(() => {
                try
                {
                    Service.Add(ConfigFilePath, true);
                }
                catch(Exception ex)
                {
                    File.WriteAllText(ErrorFilePath, ex.Message);
                }
            });

            WireguardManager.CallTunnelStateChange(WgTunnelState.Up);
        }

        public static async Task DisconnectFromRelay()
        {
            await Task.Run(() =>
            {
                Service.Remove(ConfigFilePath, true);
                try { File.Delete(ConfigFilePath); } catch { }
            });

            WireguardManager.CallTunnelStateChange(WgTunnelState.Down);
        }
    }
}
