using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Models;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : Controller
    {
        private readonly IConfiguration _config;
        private readonly WebContext _context;

        public AuthenticateController(IConfiguration config, WebContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("create-device")]
        public async Task<IActionResult> CreateDevice([FromForm] ulong userId, [FromForm] string publicKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
                return BadRequest("Invalid client request");

            if (_context.Users == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);

            if (user == null)
                return NotFound();

            int deviceCount = await _context.Devices.Where((device) => device.UserUUID == user.UUID).CountAsync();

            if (deviceCount >= int.Parse(_config["MaxDevicesPerUser"]))
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Device limit reached" });

            var deviceUUID = Guid.NewGuid();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, deviceUUID.ToString())
            };

            var accessToken = CreateAccessToken(authClaims);
            var refreshToken = CreateRefreshToken(64);

            int refreshTokenLifeTime = int.Parse(_config["JWT:RefreshLifeTimeInDays"]);

            Device device = new Device
            {
                UUID = deviceUUID,
                UserUUID = user.UUID,
                PublicKey = publicKey,
                Name = DeviceNames.GetRandomName(),

                IPV4Address = string.Empty,
                CreatedTimeStamp = DateTime.Now,
                RefreshToken = refreshToken
            };

            var deviceEntry = await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
            await deviceEntry.ReloadAsync();

            var response = new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
                Device = new
                {
                    Name = device.Name,
                    IPV4Address = device.IPV4Address
                }
            };

            return Json(response);
        }

        [HttpPost("remove-device")]
        [Authorize]
        public async Task<IActionResult> RemoveDevice()
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromForm] string accessToken, [FromForm] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("Invalid client request");

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
                return BadRequest("Invalid access token or refresh token");

            if (_context.Devices == null)
                return NotFound();

            var device = await _context.Devices.FindAsync(Guid.Parse(principal.Identity.Name));

            if (device == null || device.RefreshToken != refreshToken)
                return BadRequest("Invalid access token or refresh token");

            var newAccessToken = CreateAccessToken(principal.Claims.ToList());
            var newRefreshToken = CreateRefreshToken(64);

            device.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            var response = new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };

            return Json(response);
        }

        private JwtSecurityToken CreateAccessToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            int tokenValidityInMinutes = int.Parse(_config["JWT:AccessLifeTimeInMinutes"]);

            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                notBefore: now,
                expires: now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string CreateRefreshToken(int byteLenght)
        {
            var randomNumber = new byte[byteLenght];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
