using Newtonsoft.Json;

namespace WebAPI.Relays.Type
{
    public class City
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string CityCode { get; set; }
        [JsonProperty("relays")]
        public List<Relay> Relays { get; set; }
    }
}
