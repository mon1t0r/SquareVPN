using Newtonsoft.Json;

namespace VPNClient_Windows_Test.Utils.SessionUtils
{
    internal class SessionManager
    {
        private const string SavePath = "session.json";

        public static Session? CurrentSession { get; set; }

        public static void SaveSession() =>
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(CurrentSession));

        public static void LoadSession()
        {
            if (File.Exists(SavePath))
                CurrentSession = JsonConvert.DeserializeObject<Session>(File.ReadAllText(SavePath));
        }
    }
}
