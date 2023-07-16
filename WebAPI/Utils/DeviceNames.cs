using Newtonsoft.Json;

namespace WebAPI.Utils
{
    public class DeviceNames
    {
        private static DeviceNames Instance { get; set; }

        [JsonProperty("FirstNames")]
        private List<string> FirstNames { get; set; }
        [JsonProperty("SecondNames")]
        private List<string> SecondNames { get; set; }

        public static void Initialize()
        {
            Instance = JsonConvert.DeserializeObject<DeviceNames>(File.ReadAllText("devicenames.json"));
        }

        public static string GetRandomName()
        {
            Random rand = new Random();
            return Instance.FirstNames[rand.Next(Instance.FirstNames.Count)] + " " +
                Instance.SecondNames[rand.Next(Instance.SecondNames.Count)];
        }
    }
}
