using VPNServer.Classes;
using VPNServer.Utils;

namespace VPNServer
{
    public class Program
    {
        private static List<Peer> ConnectedPeers = new();

        public static async Task Main(string[] args)
        {
            do
            {
                IEnumerable<string> result = await ConsoleUtils.ExecuteCommand("wg");
                IEnumerable<Peer> peers = ParsingUtils.ParsePeersFromWG(result);
                foreach(Peer peer in peers)
                    Console.WriteLine(peer.ToString());
                Thread.Sleep(1000);
            } while (true);

            Console.ReadLine();
        }
    }
}