using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace API.Responses.Models.Relays
{
    public class APIRelayBase
    {
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("ipv4")]
        public string IPV4 { get; set; }

        public override string ToString() => Hostname;
    }
}
