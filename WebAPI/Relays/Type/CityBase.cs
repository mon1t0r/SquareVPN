using Newtonsoft.Json;

namespace WebAPI.Relays.Type
{
    public abstract class CityBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string CityCode { get; set; }
    }
}
