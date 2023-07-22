using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Responses.Models.Relays
{
    public class APICityBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string CityCode { get; set; }

        public override string ToString() => Name;
    }
}
