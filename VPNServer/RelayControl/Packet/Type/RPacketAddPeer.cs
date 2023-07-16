using System.Net;
using VPNServer.Classes;
using VPNServer.Utils;

namespace VPNServer.RelayControl.Packet.Type
{
    public class RPacketAddPeer
    {
        public byte[] PublicKey { get; set; }
        public IPAddress IPV4Address { get; set; }

        public RPacketAddPeer(byte[] publicKey, IPAddress ipv4Address)
        {
            PublicKey = publicKey;
            IPV4Address = ipv4Address;
        }

        public static object Decoder(BinaryReader reader)
        {
            byte[] publicKey = reader.ReadBytes(reader.ReadInt32());
            byte[] addressBytes = reader.ReadBytes(reader.ReadInt32());
            return new RPacketAddPeer(publicKey, new IPAddress(addressBytes));
        }

        public static async void Handler(object obj)
        {
            RPacketAddPeer packet = (RPacketAddPeer)obj;
            await CommandUtils.AddPeer(new Peer
            {
                PublicKey = packet.PublicKey,
                IPV4Address = packet.IPV4Address
            });
        }
    }
}
