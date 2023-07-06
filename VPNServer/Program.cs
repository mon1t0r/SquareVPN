using System.Collections.Generic;
using System.Net;
using VPNServer.Classes;
using VPNServer.Utils;

namespace VPNServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Thread peerUpdateThread = new Thread(StartPeerUpdateThread);
            peerUpdateThread.Start();

            Console.ReadLine();
        }

        private static async void StartPeerUpdateThread()
        {
            do
            {
                Console.WriteLine("Update");
                IEnumerable<string> result = await CommandUtils.ExecuteCommandWithOutput("wg");
                IEnumerable <Peer> peers = ParsingUtils.ParsePeersFromWG(result);
                foreach (Peer peer in peers)
                    if ((DateTime.Now - peer.LatestHandshakeTimestamp).TotalSeconds > 10)
                        await CommandUtils.RemovePeer(peer);

                await CommandUtils.AddPeer(new Peer
                {
                    PublicKey = Convert.FromBase64String("iheCCfc8tsQVof3eOru6d/MiOeFlG11p5vYF9PlLXCY="),
                    IPAddress = IPAddress.Parse("10.66.66.2")
                });

                Thread.Sleep(15000);//180000
            } while (true);
        }
    }
}