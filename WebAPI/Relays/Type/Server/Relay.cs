﻿using Newtonsoft.Json;
using System.Net;
using WebAPI.Relays.Type.Client;

namespace WebAPI.Relays.Type.Server
{
    public class Relay : RelayBase
    {
        [JsonProperty("private_ipv4")]
        public IPAddress PrivateIPV4 { get; set; }
    }
}
