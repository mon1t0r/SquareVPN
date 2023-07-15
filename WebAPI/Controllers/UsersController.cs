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
    public class UsersController : ControllerBase
    {
        private readonly WebContext _context;

        public UsersController(WebContext context)
        {
            _context = context;
        }

        [HttpGet]
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
        }

        [HttpPost]
        public async Task<ActionResult<User>> ConnectUser(ulong id)
        {
            if (_context.Users == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            PacketManager.SendPacketToRelay(new RPacketAddPeer(Convert.FromBase64String("iheCCfc8tsQVof3eOru6d/MiOeFlG11p5vYF9PlLXCY="), IPAddress.Parse("10.0.0.3")), RelayManager.Relays[0]);

            return Ok();
        }
    }
}
