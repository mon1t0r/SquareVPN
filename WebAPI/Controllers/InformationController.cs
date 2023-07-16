using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Relays;
using WebAPI.Relays.Type;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("info")]
    public class InformationController : ControllerBase
    {
        [HttpGet("/relays")]
        public async Task<ActionResult<string>> GetRelays()
        {
            return RelayManager.CountriesJson;
        }
    }
}
