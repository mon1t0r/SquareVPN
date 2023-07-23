using Newtonsoft.Json;

namespace VPNServer.Utils
{
    public class Config
    {
        [JsonIgnore]
        public static Config Instance { get; private set; }

        [JsonProperty("ControlServerAddress")]
        public string ControlServerAddress { get; private set; }
        [JsonProperty("ControlServerPort")]
        public int ControlServerPort { get; private set; }
        [JsonProperty("PeerRemoveIntervalSeconds")]
        public int PeerRemoveIntervalSeconds { get; private set; }

        public static void Initialize()
        {
            Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText("relaysettings.json"));
        }
    }
}
