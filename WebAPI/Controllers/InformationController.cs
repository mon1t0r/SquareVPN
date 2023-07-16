using Microsoft.AspNetCore.Mvc;
using WebAPI.Relays;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("info")]
    public class InformationController : ControllerBase
    {
        [HttpGet("relays")]
        public async Task<ActionResult<string>> GetRelays()
        {
            return RelayManager.ClientCountriesJson;
        }
    }
}
