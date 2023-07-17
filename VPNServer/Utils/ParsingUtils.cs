using System.Net;
using VPNServer.Classes;

namespace VPNServer.Utils
{
    public class ParsingUtils
    {
        public static IEnumerable<Peer> ParsePeersFromWG(IEnumerable<string> inputLines)
        {
            var peersList = new List<Peer>();

            Peer? curPeer = null;
            foreach (string line in inputLines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("interface"))
                {
                    if (curPeer != null) peersList.Add(curPeer);
                    curPeer = null;
                    continue;
                }

                if (line.StartsWith("peer"))
                {
                    curPeer = new Peer();
                    curPeer.PublicKey = Convert.FromBase64String(line.Replace("peer:", null).Trim());
                    continue;
                }

                if (curPeer == null) continue;

                string str = line.Trim();

                if (str.StartsWith("allowed ips:"))
                {
                    IPAddress? ipAddress;
                    if (IPAddress.TryParse(str.Replace("allowed ips:", null).Replace("/32", null).Trim(), out ipAddress))
                        curPeer.IPV4Address = ipAddress;
                    continue;
                }
                if (str.StartsWith("latest handshake:"))
                {
                    int h = 0, m = 0, s = 0;

                    int index;
                    if ((index = str.IndexOf("seconds")) != -1 || (index = str.IndexOf("second")) != -1)
                        s = int.Parse(str.Substring(index - 3, 2));

                    if ((index = str.IndexOf("minutes")) != -1 || (index = str.IndexOf("minute")) != -1)
                        m = int.Parse(str.Substring(index - 3, 2));

                    if ((index = str.IndexOf("hours")) != -1 || (index = str.IndexOf("hour")) != -1)
                        h = int.Parse(str.Substring(index - 3, 2));

                    curPeer.LatestHandshakeTimestamp = DateTime.Now - TimeSpan.FromSeconds(h * 3600 + m * 60 + s);
                }
            }

            if (curPeer != null) peersList.Add(curPeer);

            return peersList;
        }
    }
}
