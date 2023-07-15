using NetCoreServer;
using System.Text;
using VPNServer.RelayControl.Packet;

namespace VPNServer.RelayControl
{
    public class ControlClient : TcpClient
    {
        public ControlClient(string address, int port) : base(address, port) { }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
                Thread.Yield();
        }

        protected override void OnConnected() =>
            Console.WriteLine($"Relay TCP client connected a new session with Id {Id}");

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Relay TCP client disconnected a session with Id {Id}");
            Thread.Sleep(1000);
            if (!_stop)
                ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size) =>
            PacketManager.HandleRelayPacket(new BinaryReader(new MemoryStream(buffer, (int)offset, (int)size)));

        protected override void OnError(System.Net.Sockets.SocketError error) =>
            Console.WriteLine($"Relay TCP client caught an error with code {error}");

        private bool _stop;
    }
}
