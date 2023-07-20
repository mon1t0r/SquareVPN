using API;
using Newtonsoft.Json;

namespace VPNClient_Windows_Test.Utils
{
    internal class SessionManager
    {
        private const string SavePath = "session.json";

        public static APISession? CurrentSession { get; set; }

        public static void SaveSession() =>
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(CurrentSession));

        public static void LoadSession()
        {
            if (File.Exists(SavePath))
                CurrentSession = JsonConvert.DeserializeObject<APISession>(File.ReadAllText(SavePath));
        }
    }
}
