using Newtonsoft.Json;
using API;
using API.Utils;
using VPNClient.Pages;

namespace VPNClient.Classes
{
    public class SessionManager
    {
        private const string SessionKey = "session";
        private const string SessionEndpoint = "https://localhost:44317/";

        public static APISession CurrentSession { get; private set; }

        public static void Initialize()
        {
            CurrentSession = new APISession(SessionEndpoint);
            CurrentSession.OnDataUpdated += CurrentSession_OnDataUpdated;
            CurrentSession.OnLogout += CurrentSession_OnLogout;
        }

        private static async void CurrentSession_OnDataUpdated() =>
            await SaveSessionAsync();

        private static void CurrentSession_OnLogout() =>
            Application.Current.MainPage = new NavigationPage(new LoginPage());

        public static async Task SaveSessionAsync()
        {
            if (CurrentSession != null)
                await SecureStorage.Default.SetAsync(SessionKey, JsonConvert.SerializeObject(CurrentSession));
            else
                SecureStorage.Default.Remove(SessionKey);
        }

        public static async Task LoadSessionAsync()
        {
            var value = await SecureStorage.Default.GetAsync(SessionKey);
            if(value != null)
                CurrentSession = JsonConvert.DeserializeObject<APISession>(value);
            else
                CurrentSession = new APISession();

            CurrentSession.APIEndpoints = new APIEndpoints(SessionEndpoint);
        }
    }
}
