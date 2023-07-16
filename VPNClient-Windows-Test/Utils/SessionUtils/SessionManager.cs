using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VPNClient_Windows_Test.Utils.SessionUtils.Response;

namespace VPNClient_Windows_Test.Utils.SessionUtils
{
    internal class SessionManager
    {
        private const string SavePath = "session.json";
        public const string APIEndpoint = "https://localhost:44317/";
        private static readonly HttpClient HttpClient = new HttpClient();

        public static Session? CurrentSession { get; set; }

        public static async Task CreateSession(ulong userId)
        {
            if (CurrentSession != null)
                CurrentSession = null;

            var keys = WireguardKeyUtils.GenKeyPair();
            string privateKeyStr = keys.Item1;
            string publicKeyStr = keys.Item2;

            var requestData = new Dictionary<string, string>
            {
                { "userId", userId.ToString() },
                { "publicKey", publicKeyStr }
            };

            var response = await HttpClient.PostAsync(new Uri(APIEndpoint + "auth/create-device"), new FormUrlEncodedContent(requestData));

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return;

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (authResponse == null)
                return;

            CurrentSession = new Session
            {
                AccessToken = authResponse.AccessToken,
                RefreshToken = authResponse.RefreshToken,
                DeviceName = authResponse.Device.Name,
                IPV4Address = authResponse.Device.IPV4Address,
                PrivateKey = privateKeyStr
            };
        }

        public static async Task RemoveSession()
        {
            if (CurrentSession == null)
                return;

            var request = new HttpRequestMessage(HttpMethod.Post, APIEndpoint + "auth/remove-device");
            request.Headers.Add("Authorization", $"Bearer {CurrentSession.AccessToken}");
            var response = await HttpClient.SendAsync(request);

            if (response == null || response.Content == null)
                return;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();
                MessageBox.Show("Access token was updated, please call this method again", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CurrentSession = null;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return;
        }

        public static async Task RefreshAccessToken()
        {
            if (CurrentSession == null)
                return;

            var requestData = new Dictionary<string, string>
            {
                { "accessToken", CurrentSession.AccessToken },
                { "refreshToken", CurrentSession.RefreshToken }
            };

            var response = await HttpClient.PostAsync(new Uri(APIEndpoint + "auth/refresh-token"), new FormUrlEncodedContent(requestData));

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return;

            var refreshResponse = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
            if (refreshResponse == null)
                return;

            CurrentSession.AccessToken = refreshResponse.AccessToken;
            CurrentSession.RefreshToken = refreshResponse.RefreshToken;
        }

        public static async Task ConnectPeer(string hostname)
        {
            if (CurrentSession == null)
                return;

            var request = new HttpRequestMessage(HttpMethod.Post, APIEndpoint + "relays/connect-peer");
            
            request.Headers.Add("Authorization", $"Bearer {CurrentSession.AccessToken}");

            var requestData = new Dictionary<string, string>
            {
                { "hostname", hostname }
            };

            request.Content = new FormUrlEncodedContent(requestData);

            var response = await HttpClient.SendAsync(request);

            if (response == null || response.Content == null)
                return;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();
                MessageBox.Show("Access token was updated, please call this method again", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public static async Task<string> GetRelays()
        {
            var response = await HttpClient.GetAsync(new Uri(APIEndpoint + "info/relays"));

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return string.Empty;

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<DateTime?> GetPaidUntil()
        {
            if (CurrentSession == null)
                return null;

            var request = new HttpRequestMessage(HttpMethod.Get, APIEndpoint + "info/paid-until");

            request.Headers.Add("Authorization", $"Bearer {CurrentSession.AccessToken}");

            var response = await HttpClient.SendAsync(request);

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return null; ;

            return DateTime.FromBinary(long.Parse(await response.Content.ReadAsStringAsync()));
        }

        public static void SaveSession() =>
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(CurrentSession));

        public static void LoadSession()
        {
            if (File.Exists(SavePath))
                CurrentSession = JsonConvert.DeserializeObject<Session>(File.ReadAllText(SavePath));
            else
                CurrentSession = new Session();
        }
    }
}
