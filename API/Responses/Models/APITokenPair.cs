using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Responses.Models
{
    public class APITokenPair
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
