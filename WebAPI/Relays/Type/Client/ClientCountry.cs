using Newtonsoft.Json;

namespace WebAPI.Relays.Type.Client
{
    public class ClientCountry : CountryBase
    {
        [JsonProperty("cities")]
        public List<ClientCity> Cities { get; set; }
    }
}
