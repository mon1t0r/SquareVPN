using Newtonsoft.Json;

namespace WebAPI.Relays.Type
{
    public abstract class CountryBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string CountryCode { get; set; }
    }
}
