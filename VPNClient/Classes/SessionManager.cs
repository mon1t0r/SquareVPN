using Newtonsoft.Json;
using API;
using API.Utils;
using VPNClient.Pages;
using API.Responses.Models.Relays;

namespace VPNClient.Classes
{
    public class SessionManager
    {
        private const string SessionKey = "session";
        private const string SessionEndpoint = "http://192.168.2.110:61995/";

        public static APISession CurrentSession { get; private set; }

        public static DateTime? PaidUntil { get; private set; }
        public static List<APICountry> Countries { get; private set; }

        public static async Task Initialize()
        {
            await LoadSessionAsync();
            RegisterSessionEvents();
            await UpdatePaidUntilAsync();
            await UpdateCountriesAsync();
        }

        private static async Task CurrentSession_OnDataUpdated() =>
            await SaveSessionAsync();

        private static async Task CurrentSession_OnLogin()
        {
            await UpdatePaidUntilAsync();
            await UpdateCountriesAsync();
        }

        private static Task CurrentSession_OnLogout()
        {
            if (Application.Current != null)
                Application.Current.MainPage = new LoginPage();
            PaidUntil = null;
            Countries = null;

            return Task.CompletedTask;
        }

        private static async Task UpdatePaidUntilAsync()
        {
            if (CurrentSession.IsActive)
            {
                PaidUntil = await CurrentSession.GetPaidUntil();
                if (PaidUntil != null)
                    PaidUntil = PaidUntil.Value.ToLocalTime();
            }
        }

        private static async Task UpdateCountriesAsync()
        {
            if (CurrentSession.IsActive)
                Countries = await CurrentSession.GetRelays();
        }

        private static async Task SaveSessionAsync()
        {
            if (CurrentSession.IsActive)
                await SecureStorage.Default.SetAsync(SessionKey, JsonConvert.SerializeObject(CurrentSession));
            else
                SecureStorage.Default.Remove(SessionKey);
        }

        private static async Task LoadSessionAsync()
        {
            var value = await SecureStorage.Default.GetAsync(SessionKey);
            if(value != null)
                CurrentSession = JsonConvert.DeserializeObject<APISession>(value);
            else
                CurrentSession = new APISession();

            CurrentSession.APIEndpoints = new APIEndpoints(SessionEndpoint);
        }

        private static void RegisterSessionEvents()
        {
            CurrentSession.OnDataUpdated = CurrentSession_OnDataUpdated;
            CurrentSession.OnLogin = CurrentSession_OnLogin;
            CurrentSession.OnLogout = CurrentSession_OnLogout;
        }
    }
}
