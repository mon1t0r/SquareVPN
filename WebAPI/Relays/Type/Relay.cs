using Newtonsoft.Json;
using System.Net;

namespace WebAPI.Relays.Type
{
    public class Relay
    {
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("public_ipv4")]
        public IPAddress PublicIPV4 { get; set; }
        [JsonProperty("private_ipv4")]
        public IPAddress PrivateIPV4 { get; set; }
    }
}
