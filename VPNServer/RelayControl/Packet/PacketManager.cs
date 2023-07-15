using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VPNServer.RelayControl.Packet.Type;

namespace VPNServer.RelayControl.Packet
{
    public class PacketManager
    {
        private static short Id = 0;
        private static readonly Dictionary<System.Type, short> ServerPacketDict = new();
        private static readonly Dictionary<short, RelayPacket> RelayPacketDict = new();

        public static void RegisterPackets()
        {
            ServerPacketDict.Clear();
            RelayPacketDict.Clear();
            RegisterRelayPacket(RPacketAddPeer.Decoder, RPacketAddPeer.Handler);
        }

        private static void RegisterServerPacket(System.Type packet) =>
            ServerPacketDict.Add(packet, Id++);

        private static void RegisterRelayPacket(Func<BinaryReader, object> decoder, Action<object> handler) =>
            RelayPacketDict.Add(Id++, new RelayPacket(decoder, handler));

        public static void HandleRelayPacket(BinaryReader data)
        {
            try
            {
                short id = data.ReadInt16();
                if (RelayPacketDict.TryGetValue(id, out RelayPacket packet))
                {
                    object decoded = packet.Decode(data);
                    Thread packetThread = new(() => packet.Handle(decoded));
                    packetThread.Start();

                    if (data.BaseStream.Position < data.BaseStream.Length)
                        HandleRelayPacket(data);
                    return;
                }
                throw new Exception("Unknown message type.");
            }
            catch (Exception ex) { Debug.Fail("Error while handling relay packet: " + ex.Message); }
        }

        public static void SendPacketToServer(IServerPacket packet)
        {
            MemoryStream stream = new MemoryStream();
            WriteServerPacketToStream(packet, ref stream);
            ControlClientManager.SendMessageToServer(stream.ToArray());
            stream.Close();
        }
        private static void WriteServerPacketToStream(IServerPacket packet, ref MemoryStream stream)
        {
            if (ServerPacketDict.TryGetValue(packet.GetType(), out short index))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(index);
                packet.WriteToStream(writer);
            }
        }

        private class RelayPacket
        {
            private Func<BinaryReader, object> Decoder { get; }
            private Action<object> Handler { get; }

            public RelayPacket(Func<BinaryReader, object> decoder, Action<object> handler)
            {
                Decoder = decoder;
                Handler = handler;
            }

            public object Decode(BinaryReader reader) => Decoder.Invoke(reader);

            public void Handle(object packet) => Handler.Invoke(packet);
        }
    }
}
