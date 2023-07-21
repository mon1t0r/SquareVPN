using API.Responses;
using API.Responses.Models;
using API.Utils;
using Newtonsoft.Json;
using System.Net;

namespace API
{
    public class APISession
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public APITokenPair? TokenPair { get; set; }
        public APIDevice? Device { get; set; }
        public string? PrivateKey { get; set; }

        public delegate void DataUpdatedEventHandler();
        public event DataUpdatedEventHandler OnDataUpdated;

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

            var authResponse = JsonConvert.DeserializeObject<APICreateDeviceResponse>(await response.Content.ReadAsStringAsync());
            if (authResponse == null)
                return false;

            if (authResponse.Status == "Success")
            {
                var responseData = JsonConvert.DeserializeObject<APICreateDeviceResponseData>(authResponse.Data);
                if (responseData != null)
                {
                    TokenPair = responseData.TokenPair;
                    Device = responseData.Device;
                    PrivateKey = keyPair.Item1;

                    OnDataUpdated.Invoke();

                    return true;
                }
            }
            if (authResponse.Status == "RemoveDevice")
            {
                maxDevicesCallback.Invoke(JsonConvert.SerializeObject(authResponse.Data, Formatting.Indented));
                return false;
            }

            return false;
        }

        public async Task<bool> Logout()
        {
            var response = await SendRequestAsync(HttpMethod.Post, APIEndpoints.RemoveCurrentDevice);

            if (response == null || response.Content == null)
                return false;

            if(response.IsSuccessStatusCode)
            {
                TokenPair = null;
                Device = null;
                PrivateKey = null;

                OnDataUpdated.Invoke();

                return true;
            }

            return false;
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

            var paidUntilResponse = JsonConvert.DeserializeObject<APIPaidUntilResponse>(await response.Content.ReadAsStringAsync());
            if (paidUntilResponse == null)
                return null;

            return paidUntilResponse.PaidUntilUTC;
        }

        public async Task<bool> RefreshAccessToken()
        {
            if (TokenPair == null || string.IsNullOrWhiteSpace(TokenPair.AccessToken) || string.IsNullOrWhiteSpace(TokenPair.RefreshToken))
                return false;

            var requestData = new Dictionary<string, string>
            {
                { "accessToken", TokenPair.AccessToken },
                { "refreshToken", TokenPair.RefreshToken }
            };

            var response = await SendRequestAsync(HttpMethod.Post, APIEndpoints.RefreshToken, requestData, false);

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return false;

            var str = await response.Content.ReadAsStringAsync();

            var refreshResponse = JsonConvert.DeserializeObject<APITokenPair>(str);
            if (refreshResponse == null)
                return false;

            TokenPair = refreshResponse;

            OnDataUpdated.Invoke();

            return true;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, Uri endpoint, Dictionary<string, string>? requestData = null, bool authorize = true)
        {
            authorize &= TokenPair != null && !string.IsNullOrWhiteSpace(TokenPair.AccessToken);

            var request = new HttpRequestMessage(httpMethod, endpoint);

            if (authorize)
                request.Headers.Add("Authorization", $"Bearer {TokenPair.AccessToken}");
            if (requestData != null)
                request.Content = new FormUrlEncodedContent(requestData);

            var response = await HttpClient.SendAsync(request);

            if (authorize && response.StatusCode == HttpStatusCode.Unauthorized && await RefreshAccessToken())
                return await SendRequestAsync(httpMethod, endpoint, requestData);

            return response;
        }
    }
}