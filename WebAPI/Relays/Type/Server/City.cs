using Newtonsoft.Json;

namespace WebAPI.Relays.Type.Server
{
    public class City : CityBase
    {
        [JsonProperty("relays")]
        public List<Relay> Relays { get; set; }
    }
}
