using Newtonsoft.Json;

namespace WebAPI.Relays.Type.Client
{
    public class ClientCity : CityBase
    {
        [JsonProperty("relays")]
        public List<ClientRelay> Relays { get; set; }
    }
}
