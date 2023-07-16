using NetCoreServer;
using System.Net;
using System.Net.Sockets;
using WebAPI.Relays.Type.Server;

namespace WebAPI.Relays.ControlServer
{
    public class ControlServer : TcpServer
    {
        public ControlServer(IPAddress address, int port) : base(address, port) { }

        public TcpSession? FindSessionForRelay(Relay relay)
        {
            foreach (TcpSession session in Sessions.Values)
            {
                IPEndPoint? remoteEndPoint = session.Socket.RemoteEndPoint as IPEndPoint;
                if (remoteEndPoint == null)
                    continue;
                if (remoteEndPoint.Address.Equals(relay.PrivateIPV4))
                    return session;
            }
            return null;
        }

        protected override TcpSession CreateSession() { return new ControlSession(this); }

        protected override void OnError(SocketError error) =>
            Console.WriteLine($"Control TCP server caught an error with code {error}");

        protected override void OnConnecting(TcpSession session)
        {
            IPEndPoint? remoteEndPoint = session.Socket.RemoteEndPoint as IPEndPoint;
            if (remoteEndPoint != null)
            {
                foreach (var relay in RelayManager.Relays)
                    if (remoteEndPoint.Address.Equals(relay.PrivateIPV4))
                        return;
            }
            session.Socket.Close();
        }
    }
}
