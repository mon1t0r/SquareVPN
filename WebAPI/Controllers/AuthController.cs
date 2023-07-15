using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly WebContext _context;

        public AuthController(IConfiguration config, WebContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("/token")]
        public IActionResult GenerateToken(ulong userId)
        {
            if (_context.Users == null)
                return NotFound();
            var user = _context.Users.Where(c => c.Id == userId).FirstOrDefault();

            if (user == null)
                return NotFound();

            var identity = GetIdentity(user);

            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(int.Parse(_config["Jwt:LifeTime"]))),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256));
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                uuid = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, user.UUID.ToString()) };
            ClaimsIdentity claimsIdentity = new (claims, "Token", ClaimsIdentity.DefaultNameClaimType, null);
            return claimsIdentity;
        }
    }
}
