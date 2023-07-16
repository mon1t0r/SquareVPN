using Newtonsoft.Json;

namespace WebAPI.Relays.Type.Server
{
    public class Country : CountryBase
    {
        [JsonProperty("cities")]
        public List<City> Cities { get; set; }
    }
}
