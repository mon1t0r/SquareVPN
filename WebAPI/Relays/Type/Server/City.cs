using API.Responses.Models.Relays;
using Newtonsoft.Json;

namespace WebAPI.Relays.Type.Server
{
    public class City : APICityBase
    {
        [JsonProperty("relays")]
        public List<Relay> Relays { get; set; }
    }
}
