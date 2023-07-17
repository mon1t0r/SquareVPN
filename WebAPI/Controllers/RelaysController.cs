using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Models;
using WebAPI.Relays;
using WebAPI.Relays.ControlServer.Packet;
using WebAPI.Relays.ControlServer.Packet.Type;
using WebAPI.Relays.Type.Server;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("relays")]
    [Authorize]
    public class RelaysController : ControllerBase
    {
        private readonly WebContext _context;

        public RelaysController(WebContext context)
        {
            _context = context;
        }

        [HttpPost("connect-peer")]
        public async Task<ActionResult<User>> ConnectPeer([FromForm] string hostname)
        {
            if (_context.Devices == null || _context.Users == null)
                return NotFound();

            var device = await _context.Devices.FindAsync(Guid.Parse(User.Identity.Name));

            if (device == null)
                return NotFound();

            var user = await _context.Users.FindAsync(device.UserUUID);

            if (user == null)
                return NotFound();

            if (user.PaidUntilTimeStamp < DateTime.UtcNow)
                return BadRequest("Not paid");

            if (string.IsNullOrWhiteSpace(hostname))
                return BadRequest("Invalid hostname");

            Relay? relay = RelayManager.Relays.Find((r) => r.Hostname == hostname);

            if (relay == null)
                return BadRequest("Hostname not found");

            PacketManager.SendPacketToRelay(new RPacketAddPeer(Convert.FromBase64String(device.PublicKey), IPAddress.Parse(device.IPV4Address)), relay);

            return Ok();
        }
    }
}
