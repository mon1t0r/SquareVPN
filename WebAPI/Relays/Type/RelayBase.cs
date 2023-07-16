using Newtonsoft.Json;
using System.Net;

namespace WebAPI.Relays.Type
{
    public abstract class RelayBase
    {
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("ipv4")]
        public IPAddress IPV4 { get; set; }
    }
}
