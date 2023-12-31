﻿using API.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly WebContext _context;

        public UserController(WebContext context)
        {
            _context = context;
        }

        [HttpGet("paid-until")]
        public async Task<IActionResult> GetPaidTimeStamp()
        {
            if (_context.Devices == null || _context.Users == null)
                return NotFound();

            var device = await _context.Devices.FindAsync(Guid.Parse(User.Identity.Name));

            if (device == null)
                return NotFound();

            var user = await _context.Users.FindAsync(device.UserUUID);

            if (user == null)
                return NotFound();

            var response = new APIPaidUntilResponse
            {
                PaidUntilUTC = user.PaidUntilTimeStamp
            };

            return Json(response);
        }

        [HttpPost("remove-current-device")]
        public async Task<IActionResult> RemoveCurrentDevice()
        {
            if (_context.Devices == null)
                return NotFound();

            var device = await _context.Devices.FindAsync(Guid.Parse(User.Identity.Name));

            if (device == null)
                return NotFound();

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("remove-device")]
        public async Task<IActionResult> RemoveDevice([FromForm] Guid deviceUUID)
        {
            if (_context.Devices == null)
                return NotFound();

            var device = await _context.Devices.FindAsync(Guid.Parse(User.Identity.Name));

            if (device == null)
                return NotFound();

            var removeDevice = await _context.Devices.FindAsync(deviceUUID);

            if (device.UserUUID != removeDevice.UserUUID)
                return NotFound();

            _context.Devices.Remove(removeDevice);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
