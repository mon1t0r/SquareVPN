using API.Responses.Models.Relays;
using System.Diagnostics;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using Tunnel;
using VPNClient.Classes;

namespace VPNClient
{
    public class WireguardManagerWindows
    {
        private static readonly string BaseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private static readonly string ServiceFilePath = Path.Combine(BaseDirectory, "VPNClient-WindowsVPNService.exe");
        private static readonly string ConfigFilePath = Path.Combine(BaseDirectory, "SquareVPN.conf");

        public static async Task ConnectToRelay(APIRelay relay)
        {
            try { File.Delete(ConfigFilePath); } catch { }

            var configString = $"[Interface]\nPrivateKey = {SessionManager.CurrentSession.PrivateKey}\nAddress = {SessionManager.CurrentSession.Device.IPV4Address}/32\nDNS = 8.8.8.8, 1.1.1.1\n\n[Peer]\nPublicKey = {relay.PublicKey}\nEndpoint = {relay.IPV4}:{relay.Port}\nAllowedIPs = 0.0.0.0/0\n";
            await File.WriteAllBytesAsync(ConfigFilePath, Encoding.UTF8.GetBytes(configString));

            await Task.Run(() => {
                Service.Add(ConfigFilePath, ServiceFilePath, true);
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
