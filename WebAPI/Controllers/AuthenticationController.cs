using API.Responses;
using API.Responses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _config;
        private readonly WebContext _context;

        public AuthenticationController(IConfiguration config, WebContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("create-device")]
        public async Task<IActionResult> CreateDevice([FromForm] ulong userId, [FromForm] string publicKey, [FromForm] Guid? deviceRemoveUUID = null)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
                return BadRequest("Invalid client request");

            if (_context.Users == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);

            if (user == null)
                return NotFound();

            var userDevices = _context.Devices.Where((device) => device.UserUUID == user.UUID);

            if (await userDevices.CountAsync() >= int.Parse(_config["MaxDevicesPerUser"]))
            {
                if(deviceRemoveUUID != null)
                {
                    var removeDevice = await userDevices.FirstOrDefaultAsync((device) => device.UUID == deviceRemoveUUID.Value);
                    if (removeDevice == null)
                        return NotFound();

                    _context.Devices.Remove(removeDevice);
                }
                else
                {
                    var devices = new List<APIDevice>();
                    foreach (var userDevice in userDevices)
                    {
                        devices.Add(new APIDevice
                        {
                            UUID = userDevice.UUID,
                            Name = userDevice.Name,
                            CreatedUTC = userDevice.CreatedTimeStamp
                        });
                    }

                    var responseDevices = new APICreateDeviceResponse
                    {
                        Status = "RemoveDevice",
                        Data = JsonConvert.SerializeObject(devices, Formatting.Indented)
                    };

                    return Json(responseDevices);
                }
            }

            var deviceUUID = Guid.NewGuid();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, deviceUUID.ToString())
            };

            var accessToken = CreateAccessToken(authClaims);
            var refreshToken = CreateRefreshToken(64);

            Device device = new Device
            {
                UUID = deviceUUID,
                UserUUID = user.UUID,
                PublicKey = publicKey,
                Name = DeviceNames.GetRandomName(),

                IPV4Address = string.Empty,
                CreatedTimeStamp = DateTime.UtcNow,
                RefreshToken = refreshToken
            };

            var deviceEntry = await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
            await deviceEntry.ReloadAsync();

            var response = new APICreateDeviceResponse
            {
                Status = "Success",
                Data = JsonConvert.SerializeObject(new APICreateDeviceResponseData
                {
                    TokenPair = new APITokenPair
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                        RefreshToken = refreshToken
                    },
                    Device = new APIDevice
                    {
                        UUID = deviceUUID,
                        Name = device.Name,
                        IPV4Address = device.IPV4Address
                    }
                })
            };

            return Json(response);
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

            var response = new APITokenPair
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
