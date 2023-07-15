using Newtonsoft.Json;

namespace WebAPI.Relays.Type
{
    public class Country
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string CountryCode { get; set; }
        [JsonProperty("cities")]
        public List<City> Cities { get; set; }
    }
}
