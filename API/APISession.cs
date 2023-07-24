using API.Responses;
using API.Responses.Models;
using API.Responses.Models.Relays;
using API.Utils;
using Newtonsoft.Json;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace API
{
    [JsonObject(MemberSerialization.OptIn)]
    public class APISession
    {
        private static HttpClient HttpClient = new HttpClient();

        public static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? ServerCertificateCustomValidationCallback
        {
            set
            {
                HttpClient = new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = value
                });
            }
        }

        [JsonProperty]
        public APITokenPair? TokenPair { get; set; }
        [JsonProperty]
        public APIDevice? Device { get; set; }
        [JsonProperty]
        public string? PrivateKey { get; set; }

        public bool IsActive
        {
            get => TokenPair != null && Device != null && PrivateKey != null;
        }

        public APIEndpoints APIEndpoints;

        public Func<Task>? OnDataUpdated;
        public Func<Task>? OnLogin;
        public Func<Task>? OnLogout;
        public Func<Exception, Task>? OnRequestException;

        public APISession()
        {
            
        }

        public APISession(string endpoint)
        {
            APIEndpoints = new APIEndpoints(endpoint);
        }

        public async Task<bool> Login(ulong userId, (string, string) keyPair, Action<List<APIDevice>> maxDevicesCallback, string? deviceRemoveUUID = null)
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

                    if (OnDataUpdated != null)
                        await OnDataUpdated.Invoke();
                    if(OnLogin != null)
                        await OnLogin.Invoke();

                    return true;
                }
            }
            if (authResponse.Status == "RemoveDevice")
            {
                var responseData = JsonConvert.DeserializeObject<List<APIDevice>>(authResponse.Data);
                if (responseData != null)
                    maxDevicesCallback.Invoke(responseData);
                return false;
            }

            return false;
        }

        public async Task Logout()
        {
            await SendRequestAsync(HttpMethod.Post, APIEndpoints.RemoveCurrentDevice);
            await ClearSession();
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

        public async Task<List<APICountry>?> GetRelays()
        {
            var response = await SendRequestAsync(HttpMethod.Get, APIEndpoints.Relays);

            if (response == null || response.Content == null || !response.IsSuccessStatusCode)
                return null;

            var relaysResponse = JsonConvert.DeserializeObject<List<APICountry>>(await response.Content.ReadAsStringAsync());

            return relaysResponse;
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

            if(OnDataUpdated != null)
                await OnDataUpdated.Invoke();

            return true;
        }

        private async Task<HttpResponseMessage?> SendRequestAsync(HttpMethod httpMethod, Uri endpoint, Dictionary<string, string>? requestData = null, bool authorize = true)
        {
            authorize &= TokenPair != null && !string.IsNullOrWhiteSpace(TokenPair.AccessToken);

            var request = new HttpRequestMessage(httpMethod, endpoint);

            if (authorize)
                request.Headers.Add("Authorization", $"Bearer {TokenPair.AccessToken}");
            if (requestData != null)
                request.Content = new FormUrlEncodedContent(requestData);

            HttpResponseMessage? response;

            try
            {
                response = await HttpClient.SendAsync(request);
            }
            catch(Exception ex)
            {
                if (OnRequestException != null)
                    await OnRequestException.Invoke(ex);
                return null;
            }

            if (authorize && response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if(!await RefreshAccessToken())
                {
                    await ClearSession();
                    return response;
                }
                return await SendRequestAsync(httpMethod, endpoint, requestData);
            }
                

            return response;
        }

        private async Task ClearSession()
        {
            TokenPair = null;
            Device = null;
            PrivateKey = null;

            if(OnDataUpdated != null)
                await OnDataUpdated.Invoke();
            if(OnLogout != null)
                await OnLogout.Invoke();
        }
    }
}