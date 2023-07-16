using Newtonsoft.Json;
using WebAPI.Relays.Type.Client;
using WebAPI.Relays.Type.Server;
using WebAPI.Relays.Util;

namespace WebAPI.Relays
{
    public class RelayManager
    {
        public static List<Country> Countries { get; private set; }
        public static List<Relay> Relays { get; private set; }

        public static string ClientCountriesJson { get; private set; }

        public static void Initialize()
        {
            var settings = new JsonSerializerSettings
            {
                Converters =
                {
                    new IPAddressJsonConverter()
                }
            };
            Countries = JsonConvert.DeserializeObject<List<Country>>(File.ReadAllText("relays.json"), settings);
            if (Countries == null)
                throw new Exception("Relay list was null.");

            Relays = new();
            foreach (var country in Countries)
                foreach (var city in country.Cities)
                    Relays.AddRange(city.Relays);

            var clientCountries = new List<ClientCountry>();
            foreach (var country in Countries)
            {
                var clientCities = new List<ClientCity>();
                foreach (var city in country.Cities)
                {
                    var clientRelays = new List<ClientRelay>();
                    foreach (var relay in city.Relays)
                    {
                        clientRelays.Add(new()
                        {
                            Hostname = relay.Hostname,
                            IPV4 = relay.IPV4
                        });
                    }

                    clientCities.Add(new()
                    {
                        Name = city.Name,
                        CityCode = city.CityCode,
                        Relays = clientRelays
                    });
                }

                clientCountries.Add(new()
                {
                    Name = country.Name,
                    CountryCode = country.CountryCode,
                    Cities = clientCities
                });
            }

            ClientCountriesJson = JsonConvert.SerializeObject(clientCountries, Formatting.Indented, settings);
        }
    }
}
