using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GalleryApp.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthOptions _opts;
        private readonly IUserRepository _repository;
        public AuthController(IOptions<AuthOptions> opts, IUserRepository repository)
        {
            _opts = opts.Value;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(User model)
        {
            var identity = await GetIdentity(model);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_opts.Secret));

            var jwt = new JwtSecurityToken(
                    issuer: _opts.Issuer,
                    audience: _opts.Audience,
                    claims: identity.Claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_opts.Lifetime)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
            };

            return Ok(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(User model)
        {
            User user = await _repository.LoginAsync(model);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
