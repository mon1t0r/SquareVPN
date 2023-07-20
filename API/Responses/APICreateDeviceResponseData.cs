using API.Responses.Models;

namespace API.Responses
{
    public class APICreateDeviceResponseData
    {
        public APITokenPair TokenPair { get; set; }
        public APIDevice Device { get; set; }
    }
}
