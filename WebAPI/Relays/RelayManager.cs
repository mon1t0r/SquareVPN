using API.Responses.Models.Relays;
using Newtonsoft.Json;
using WebAPI.Relays.Type.Server;
using WebAPI.Relays.Util;

namespace WebAPI.Relays
{
    public class RelayManager
    {
        public static List<Country> Countries { get; private set; }
        public static List<Relay> Relays { get; private set; }

        public static string APICountriesJson { get; private set; }

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

            var apiCountries = new List<APICountry>();
            foreach (var country in Countries)
            {
                var apiCities = new List<APICity>();
                foreach (var city in country.Cities)
                {
                    var apiRelays = new List<APIRelay>();
                    foreach (var relay in city.Relays)
                    {
                        apiRelays.Add(new()
                        {
                            Hostname = relay.Hostname,
                            IPV4 = relay.IPV4
                        });
                    }

                    apiCities.Add(new()
                    {
                        Name = city.Name,
                        CityCode = city.CityCode,
                        Relays = apiRelays
                    });
                }

                apiCountries.Add(new()
                {
                    Name = country.Name,
                    CountryCode = country.CountryCode,
                    Cities = apiCities
                });
            }

            APICountriesJson = JsonConvert.SerializeObject(apiCountries, Formatting.Indented, settings);
        }
    }
}
