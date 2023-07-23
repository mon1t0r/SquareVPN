using API.Responses.Models.Relays;
using Newtonsoft.Json;
using System.Net;

namespace WebAPI.Relays.Type.Server
{
    public class Relay : APIRelayBase
    {
        [JsonProperty("private_ipv4")]
        public IPAddress PrivateIPV4 { get; set; }
        [JsonProperty("avaliable")]
        public bool Avaliable { get; set; }
    }
}
