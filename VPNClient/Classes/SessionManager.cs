using Newtonsoft.Json;
using API;

namespace VPNClient.Classes
{
    public class SessionManager
    {
        private const string SessionKey = "session";

        public static APISession CurrentSession { get; private set; }

        public static void Initialize()
        {
            CurrentSession = new APISession();
            CurrentSession.OnDataUpdated += CurrentSession_OnDataUpdated;
        }

        private static async void CurrentSession_OnDataUpdated() => await SaveSession();

        public static async Task SaveSession()
        {
            if (CurrentSession != null)
                await SecureStorage.Default.SetAsync(SessionKey, JsonConvert.SerializeObject(CurrentSession));
            else
                SecureStorage.Default.Remove(SessionKey);
        }

        public static async Task LoadSession()
        {
            var value = await SecureStorage.Default.GetAsync(SessionKey);
            if(value != null)
                CurrentSession = JsonConvert.DeserializeObject<APISession>(value);
            else
                CurrentSession = null;
        }
    }
}
