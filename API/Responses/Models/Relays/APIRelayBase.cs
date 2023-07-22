using Newtonsoft.Json;

namespace API.Responses.Models.Relays
{
    public class APIRelayBase
    {
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("ipv4")]
        public string IPV4 { get; set; }
        [JsonProperty("port")]
        public int Port { get; set; }
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        public override string ToString() => Hostname;
    }
}
