using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using WebAPI.Models;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login(ulong userId)
        {
            if (_context.Users == null)
                return NotFound();

            var user = _context.Users.Where(c => c.Id == userId).FirstOrDefault();

            if (user == null)
                return NotFound();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UUID.ToString())
            };

            var accessToken = CreateAccessToken(authClaims);
            var refreshToken = CreateRefreshToken(64);

            int refreshTokenLifeTime = int.Parse(_config["JWT:RefreshLifeTimeInDays"]);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(refreshTokenLifeTime);

            await _context.SaveChangesAsync();

            var response = new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };

            return Json(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (_context.Users == null)
                return NotFound();

            var user = await _context.Users.FindAsync(Guid.Parse(User.Identity.Name));

            if (user == null)
                return NotFound();

            user.RefreshToken = null;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string accessToken, string refreshToken)
        {
            if(string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("Invalid client request");

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
                return BadRequest("Invalid access token or refresh token");

            if (_context.Users == null)
                return NotFound();

            var user = await _context.Users.FindAsync(Guid.Parse(principal.Identity.Name));

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
                return BadRequest("Invalid access token or refresh token");

            var newAccessToken = CreateAccessToken(principal.Claims.ToList());
            var newRefreshToken = CreateRefreshToken(64);

            user.RefreshToken = newRefreshToken;
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
