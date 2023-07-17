using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Relays;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("info")]
    public class InformationController : ControllerBase
    {
        private readonly WebContext _context;

        public InformationController(WebContext context)
        {
            _context = context;
        }

        [HttpGet("relays")]
        [Authorize]
        public ActionResult<string> GetRelays()
        {
            return RelayManager.ClientCountriesJson;
        }
    }
}
