using System.Net;
using VPNServer.Utils;

namespace VPNServer.Classes
{
    public class Peer
    {
        public byte[] PublicKey { get; set; } = Array.Empty<byte>();
        public IPAddress IPAddress { get; set; } = IPAddress.None;
        public DateTime LatestHandshakeTimestamp { get; set; } = DateTime.MinValue;

        public bool IsPeerKeyEqual(Peer peer) => ProgramUtils.UnsafeCompare(PublicKey, peer.PublicKey);

        public override string ToString() => $"Peer: {Convert.ToBase64String(PublicKey)}, {IPAddress}, {LatestHandshakeTimestamp}";
    }
}
