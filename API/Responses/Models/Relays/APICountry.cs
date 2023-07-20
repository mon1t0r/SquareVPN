using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Responses.Models.Relays
{
    public class APICountry : APICountryBase
    {
        [JsonProperty("cities")]
        public List<APICity> Cities { get; set; }
    }
}
