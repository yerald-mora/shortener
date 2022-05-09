using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using nativoshortener.api.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace nativoshortener.api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager
            , IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDTO>> Register(RegisterDTO login)
        {

            var user = new IdentityUser { UserName = login.UserName, Email = login.Email };
            var result = await _userManager.CreateAsync(user, login.Password);

            return (result.Succeeded) ? BuildToken(user.Id,user.UserName) : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login.UserName);

                return BuildToken(user.Id,user.UserName);
            }

            return BadRequest("Wrong user or password");
        }

        private TokenDTO BuildToken(string userId, string userName)
        {
            //TODO: Implement token refresh process with a 15 minutes lifetime and claims in case of information needed inside the token
            var expiration = DateTime.UtcNow.AddDays(1);
            var securityToken = GetJwtSecurityToken(expiration);

            return new TokenDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiration,
                UserName = userName
            };
        }

        private JwtSecurityToken GetJwtSecurityToken(DateTime expiration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["tokenKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(issuer: null, audience: null, claims: null, expires: expiration, signingCredentials: credentials);
        }
    }
}
