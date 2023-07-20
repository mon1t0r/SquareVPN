using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace API.Responses.Models
{
    public class APIDevice
    {
        public Guid UUID { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public string? IPV4Address { get; set; }
    }
}
