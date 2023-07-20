using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Responses.Models.Relays
{
    public class APICity : APICityBase
    {
        [JsonProperty("relays")]
        public List<APIRelay> Relays { get; set; }
    }
}
