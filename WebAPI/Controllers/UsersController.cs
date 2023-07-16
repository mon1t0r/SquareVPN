using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAPI.Models;
using WebAPI.Relays;
using WebAPI.Relays.ControlServer.Packet;
using WebAPI.Relays.ControlServer.Packet.Type;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly WebContext _context;

        public UsersController(WebContext context)
        {
            _context = context;
        }

        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
                return NotFound();
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(ulong id)
        {
            if (_context.Users == null)
                return NotFound();
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return user;
        }*/

        [HttpPost("/connectPeer")]
        public async Task<ActionResult<User>> ConnectPeer(string deviceUUID)
        {
            if (_context.Devices == null)
                return NotFound();

            var device = await _context.Devices.FindAsync(Guid.Parse(User.Identity.Name), Guid.Parse(deviceUUID));

            if (device == null)
                return NotFound();

            PacketManager.SendPacketToRelay(new RPacketAddPeer(Convert.FromBase64String(device.PublicKey), IPAddress.Parse(device.IPV4Address)), RelayManager.Relays[0]);

            return Ok();
        }
    }
}
