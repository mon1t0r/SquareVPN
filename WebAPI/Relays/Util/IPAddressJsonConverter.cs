using Newtonsoft.Json;
using System.Net;

namespace WebAPI.Relays.Util
{
    public class IPAddressJsonConverter : JsonConverter<IPAddress>
    {
        public override IPAddress? ReadJson(JsonReader reader, System.Type objectType, IPAddress? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            string? value = (string?)reader.Value;
            return value != null ? IPAddress.Parse(value) : IPAddress.None;
        }

        public override void WriteJson(JsonWriter writer, IPAddress? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value != null ? value.ToString() : string.Empty);
        }
    }
}
