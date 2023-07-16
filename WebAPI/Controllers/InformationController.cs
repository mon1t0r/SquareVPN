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
        public async Task<ActionResult<string>> GetRelays()
        {
            return RelayManager.ClientCountriesJson;
        }

        [HttpGet("paid-until")]
        [Authorize]
        public async Task<ActionResult<long>> GetPaidTimeStamp()
        {
            if (_context.Devices == null || _context.Users == null)
                return NotFound();

            var device = await _context.Devices.FindAsync(Guid.Parse(User.Identity.Name));

            if (device == null)
                return NotFound();

            var user = await _context.Users.FindAsync(device.UserUUID);

            if (user == null)
                return NotFound();

            return user.PaidUntilTimeStamp.ToBinary();
        }
    }
}
