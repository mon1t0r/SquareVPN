namespace VPNServer.RelayControl
{
    public class ControlClientManager
    {
        private static ControlClient ControlClient { get; set; }

        public static void InitializeClient(string serverAddress, int port)
        {
            ControlClient = new ControlClient(serverAddress, port);
            ControlClient.ConnectAsync();
        }

        public static void SendMessageToServer(byte[] msg) =>
            ControlClient.SendAsync(msg);
    }
}
