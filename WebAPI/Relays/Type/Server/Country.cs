using API.Responses.Models.Relays;
using Newtonsoft.Json;

namespace WebAPI.Relays.Type.Server
{
    public class Country : APICountryBase
    {
        [JsonProperty("cities")]
        public List<City> Cities { get; set; }
    }
}
