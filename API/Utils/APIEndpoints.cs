namespace API.Utils
{
    public class APIEndpoints
    {
        private readonly Uri APIEndpoint;

        public readonly Uri CreateDevice;
        public readonly Uri RefreshToken;

        public readonly Uri Relays;

        public readonly Uri ConnectPeer;

        public readonly Uri PaidUntil;
        public readonly Uri RemoveCurrentDevice;
        public readonly Uri RemoveDevice;

        public APIEndpoints(string endpoint)
        {
            APIEndpoint = new Uri(endpoint);

            CreateDevice = new(APIEndpoint, "auth/create-device");
            RefreshToken = new(APIEndpoint, "auth/refresh-token");

            Relays = new(APIEndpoint, "info/relays");

            ConnectPeer = new(APIEndpoint, "relays/connect-peer");

            PaidUntil = new(APIEndpoint, "user/paid-until");
            RemoveCurrentDevice = new(APIEndpoint, "user/remove-current-device");
            RemoveDevice = new(APIEndpoint, "user/remove-device");
        }
    }
}
