using VPNServer.Classes;
using VPNServer.RelayControl;
using VPNServer.RelayControl.Packet;
using VPNServer.Utils;

namespace VPNServer
{
    public class Program
    {
        private const int PeerUpdateIntervalMilliseconds = 180000;

        public static void Main(string[] args)
        {
            Config.Initialize();
            PacketManager.RegisterPackets();
            ControlClientManager.InitializeClient(Config.Instance.ControlServerAddress, Config.Instance.ControlServerPort);

            Thread peerUpdateThread = new(StartPeerUpdateThread);
            peerUpdateThread.Start();

            Console.ReadLine();
        }

        private static async void StartPeerUpdateThread()
        {
            do
            {
                IEnumerable<string> result = await CommandUtils.ExecuteCommandWithOutput("wg");
                IEnumerable<Peer> peers = ParsingUtils.ParsePeersFromWG(result);
                foreach (Peer peer in peers)
                    if ((DateTime.Now - peer.LatestHandshakeTimestamp).TotalMilliseconds > PeerUpdateIntervalMilliseconds)
                        await CommandUtils.RemovePeer(peer);

                Thread.Sleep(PeerUpdateIntervalMilliseconds);
            } while (true);
        }
    }
}