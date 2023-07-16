using Newtonsoft.Json;
using WebAPI.Relays.Type;
using WebAPI.Relays.Util;

namespace WebAPI.Relays
{
    public class RelayManager
    {
        public static string CountriesJson { get; private set; }
        public static List<Country> Countries { get; private set; }
        public static List<Relay> Relays { get; private set; }

        public static void Initialize()
        {
            CountriesJson = File.ReadAllText("relays.json");
            var settings = new JsonSerializerSettings
            {
                Converters =
                {
                    new IPAddressJsonConverter()
                }
            };
            Countries = JsonConvert.DeserializeObject<List<Country>>(CountriesJson, settings);
            if (Countries == null)
                throw new Exception("Relay list was null.");

            Relays = new();
            foreach (var country in Countries)
                foreach (var city in country.Cities)
                    Relays.AddRange(city.Relays);
        }
    }
}
