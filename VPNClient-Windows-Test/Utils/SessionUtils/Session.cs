using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace VPNClient_Windows_Test.Utils.SessionUtils
{
    public class Session
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? DeviceName { get; set; }
        public string? IPV4Address { get; set; }
        public string? PrivateKey { get; set; }

        public async Task<bool> Login(ulong userId, (string, string) keyPair, Action<string> maxDevicesCallback, string? deviceRemoveUUID = null)
        {
            var requestData = new Dictionary<string, string>
            {
                { "userId", userId.ToString() },
                { "publicKey", keyPair.Item2 }
            };

            if (!string.IsNullOrWhiteSpace(deviceRemoveUUID))
                requestData.Add("deviceRemoveUUID", deviceRemoveUUID);

            var response = await SendRequestAsync(HttpMethod.Post, APIEndpoints.CreateDevice, requestData, false);

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return false;

            var authResponse = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            if(authResponse == null)
                return false;

            if (authResponse.status == "Success")
            {
                AccessToken = authResponse.data.accessToken;
                RefreshToken = authResponse.data.refreshToken;
                DeviceName = authResponse.data.device.name;
                IPV4Address = authResponse.data.device.ipV4Address;
                PrivateKey = keyPair.Item1;

                return true;
            }
            if(authResponse.status == "RemoveDevice")
            {
                maxDevicesCallback.Invoke(JsonConvert.SerializeObject(authResponse.data, Formatting.Indented));
                return false;
            }

            return false;
        }

        public async Task<bool> Logout()
        {
            var response = await SendRequestAsync(HttpMethod.Post, APIEndpoints.RemoveCurrentDevice);

            if (response == null || response.Content == null)
                return false;

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ConnectPeer(string hostname)
        {
            var requestData = new Dictionary<string, string>
            {
                { "hostname", hostname }
            };

            var response = await SendRequestAsync(HttpMethod.Post, APIEndpoints.ConnectPeer, requestData);

            if (response == null || response.Content == null)
                return false;

            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetRelays()
        {
            var response = await SendRequestAsync(HttpMethod.Get, APIEndpoints.Relays);

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return string.Empty;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<DateTime?> GetPaidUntil()
        {
            var response = await SendRequestAsync(HttpMethod.Get, APIEndpoints.PaidUntil);

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return null;

            return DateTime.FromBinary(long.Parse(await response.Content.ReadAsStringAsync()));
        }

        public async Task<bool> RefreshAccessToken()
        {
            if (string.IsNullOrWhiteSpace(AccessToken) || string.IsNullOrWhiteSpace(RefreshToken))
                return false;

            var requestData = new Dictionary<string, string>
            {
                { "accessToken", AccessToken },
                { "refreshToken", RefreshToken }
            };

            var response = await SendRequestAsync(HttpMethod.Post, APIEndpoints.RefreshToken, requestData, false);

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return false;

            var refreshResponse = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            if (refreshResponse == null)
                return false;

            AccessToken = refreshResponse.accessToken;
            RefreshToken = refreshResponse.refreshToken;

            return true;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, Uri endpoint, Dictionary<string, string>? requestData = null, bool authorize = true)
        {
            authorize &= !string.IsNullOrWhiteSpace(AccessToken);

            var request = new HttpRequestMessage(httpMethod, endpoint);

            if(authorize)
                request.Headers.Add("Authorization", $"Bearer {AccessToken}");
            if(requestData != null)
                request.Content = new FormUrlEncodedContent(requestData);

            var response = await HttpClient.SendAsync(request);

            if (authorize && response.StatusCode == HttpStatusCode.Unauthorized && await RefreshAccessToken())
                return await SendRequestAsync(httpMethod, endpoint, requestData);

            return response;
        }
    }
}
