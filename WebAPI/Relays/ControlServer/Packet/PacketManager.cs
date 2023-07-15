using System.Collections.ObjectModel;
using System.Diagnostics;
using WebAPI.Models;
using WebAPI.Relays.ControlServer.Packet.Type;
using WebAPI.Relays.Type;

namespace WebAPI.Relays.ControlServer.Packet
{
    public class PacketManager
    {
        private static short Id = 0;
        private static readonly Dictionary<short, ServerPacket> ServerPacketDict = new();
        private static readonly Dictionary<System.Type, short> RelayPacketDict = new();

        public static void RegisterPackets()
        {
            ServerPacketDict.Clear();
            RelayPacketDict.Clear();
            RegisterRelayPacket(typeof(RPacketAddPeer));
        }

        private static void RegisterServerPacket(Func<BinaryReader, object> decoder, Action<object> handler) =>
            ServerPacketDict.Add(Id++, new ServerPacket(decoder, handler));

        private static void RegisterRelayPacket(System.Type packet) =>
            RelayPacketDict.Add(packet, Id++);

        public static void HandleServerPacket(BinaryReader data)
        {
            try
            {
                short id = data.ReadInt16();
                if (ServerPacketDict.TryGetValue(id, out ServerPacket packet))
                {
                    object decoded = packet.Decode(data);
                    Thread packetThread = new(() => packet.Handle(decoded));
                    packetThread.Start();

                    if (data.BaseStream.Position < data.BaseStream.Length)
                        HandleServerPacket(data);
                    return;
                }
                throw new Exception("Unknown message type.");
            }
            catch (Exception ex) { Debug.Fail("Error while handling server packet: " + ex.Message); }
        }

        public static void SendPacketToRelay(IRelayPacket packet, Relay relay)
        {
            MemoryStream stream = new MemoryStream();
            WriteRelayPacketToStream(packet, ref stream);
            ControlServerManager.SendMessageToRelay(stream.ToArray(), relay);
            stream.Close();
        }

        public static void MulticastPacket(IRelayPacket packet)
        {
            MemoryStream stream = new MemoryStream();
            WriteRelayPacketToStream(packet, ref stream);
            ControlServerManager.MulticastMessage(stream.ToArray());
            stream.Close();
        }

        private static void WriteRelayPacketToStream(IRelayPacket packet, ref MemoryStream stream)
        {
            if(RelayPacketDict.TryGetValue(packet.GetType(), out short index))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(index);
                packet.WriteToStream(writer);
            }
        }

        private class ServerPacket
        {
            private Func<BinaryReader, object> Decoder { get; }
            private Action<object> Handler { get; }

            public ServerPacket(Func<BinaryReader, object> decoder, Action<object> handler)
            {
                Decoder = decoder;
                Handler = handler;
            }

            public object Decode(BinaryReader reader) => Decoder.Invoke(reader);

            public void Handle(object packet) => Handler.Invoke(packet);
        }
    }
}
