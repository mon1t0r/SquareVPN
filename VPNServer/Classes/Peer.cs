using System.Net;
using VPNServer.Utils;

namespace VPNServer.Classes
{
    internal class Peer
    {
        public byte[] PublicKey { get; set; }
        public IPAddress AllowedIPs { get; set; }
        public DateTime LatestHandshakeTimestamp { get; set; }

        public bool EqualToPeer(Peer peer) => ProgramUtils.UnsafeCompare(PublicKey, peer.PublicKey);

        public override string ToString() => $"Peer: {Convert.ToBase64String(PublicKey)}, {AllowedIPs}, {LatestHandshakeTimestamp}";
    }
}
