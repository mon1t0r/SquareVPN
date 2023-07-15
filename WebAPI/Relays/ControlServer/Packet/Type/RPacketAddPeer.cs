using System.Net;

namespace WebAPI.Relays.ControlServer.Packet.Type
{
    public class RPacketAddPeer : IRelayPacket
    {
        public byte[] PublicKey { get; set; }
        public IPAddress IPV4Address { get; set; }

        public RPacketAddPeer(byte[] publicKey, IPAddress ipv4Address)
        {
            PublicKey = publicKey;
            IPV4Address = ipv4Address;
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(PublicKey.Length);
            writer.Write(PublicKey);
            byte[] addressBytes = IPV4Address.GetAddressBytes();
            writer.Write(addressBytes.Length);
            writer.Write(addressBytes);
        }
    }
}
