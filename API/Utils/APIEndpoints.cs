namespace API.Utils
{
    internal class APIEndpoints
    {
        public const string APIEndpoint = "https://localhost:44317/";

        public static readonly Uri CreateDevice = new(APIEndpoint + "auth/create-device");
        public static readonly Uri RefreshToken = new(APIEndpoint + "auth/refresh-token");

        public static readonly Uri Relays = new(APIEndpoint + "info/relays");

        public static readonly Uri ConnectPeer = new(APIEndpoint + "relays/connect-peer");

        public static readonly Uri PaidUntil = new(APIEndpoint + "user/paid-until");
        public static readonly Uri RemoveCurrentDevice = new(APIEndpoint + "user/remove-current-device");
        public static readonly Uri RemoveDevice = new(APIEndpoint + "user/remove-device");
    }
}
