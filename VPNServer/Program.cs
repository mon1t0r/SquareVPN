using VPNServer.Classes;
using VPNServer.RelayControl;
using VPNServer.RelayControl.Packet;
using VPNServer.Utils;

namespace VPNServer
{
    public class Program
    {
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
                Console.WriteLine("Update");
                IEnumerable<string> result = await CommandUtils.ExecuteCommandWithOutput("wg");
                IEnumerable<Peer> peers = ParsingUtils.ParsePeersFromWG(result);
                foreach (Peer peer in peers)
                    if ((DateTime.Now - peer.LatestHandshakeTimestamp).TotalSeconds > 10)
                        await CommandUtils.RemovePeer(peer);

                Thread.Sleep(180000);//180000
            } while (true);
        }
    }
}