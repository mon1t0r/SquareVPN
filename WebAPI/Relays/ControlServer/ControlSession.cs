using NetCoreServer;
using System.Net.Sockets;
using System.Text;
using WebAPI.Relays.ControlServer.Packet;

namespace WebAPI.Relays.ControlServer
{
    public class ControlSession : TcpSession
    {
        public ControlSession(TcpServer server) : base(server) { }

        protected override void OnConnected() =>
            Console.WriteLine($"Control TCP session with Id {Id} connected!");

        protected override void OnDisconnected() =>
            Console.WriteLine($"Control TCP session with Id {Id} disconnected!");

        protected override void OnReceived(byte[] buffer, long offset, long size) =>
            PacketManager.HandleServerPacket(new BinaryReader(new MemoryStream(buffer, (int)offset, (int)size)));

        protected override void OnError(SocketError error) =>
            Console.WriteLine($"Control TCP session caught an error with code {error}");
    }
}
