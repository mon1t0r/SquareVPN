﻿using NetCoreServer;
using System.Net;
using WebAPI.Relays.Type.Server;

namespace WebAPI.Relays.ControlServer
{
    public class ControlServerManager
    {
        private static ControlServer ControlServer { get; set; }

        public static void InitializeServer(int port)
        {
            ControlServer = new ControlServer(IPAddress.Any, port);
            ControlServer.Start();
        }

        public static void SendMessageToRelay(byte[] msg, Relay relay)
        {
            TcpSession? session = ControlServer.FindSessionForRelay(relay);
            session?.SendAsync(msg);
        }

        public static void MulticastMessage(byte[] msg) => ControlServer.Multicast(msg);
    }
}
