using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Financial.Presentation.ChatWebServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Financial.Presentation.ChatWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private UserManager<IdentityUser> _identityUserManager;
        private SignInManager<IdentityUser> _identitySignInManager;
        private readonly IConfiguration _configuration;

        public LoginController
        (
            UserManager<IdentityUser> identityUserManager,
            SignInManager<IdentityUser> identitySignInManager,
            IConfiguration configuration
        )
        {
            this._configuration = configuration;
            this._identitySignInManager = identitySignInManager;
            this._identityUserManager = identityUserManager;
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<object> SignIn(LoginAttemp LoginAttemp)
        {
            var user = await this._identityUserManager.FindByNameAsync(LoginAttemp.UserName);
            var singInResult = await this._identitySignInManager.PasswordSignInAsync(user, LoginAttemp.Password, true, false);
            if (singInResult.Succeeded)
            {
                return await GenerateJwtToken(user.Email, user);
            }
            else
            {
                throw new Exception($"Login failed!");
            }
        }

        [HttpPost]
        [Route("RegisterNewUser")]
        public async Task RegisterNewUser(RegisterNewUser registerNewUser)
        {
            var result = await this._identityUserManager.CreateAsync(new IdentityUser()
            {
                UserName = registerNewUser.UserName,
                Email = registerNewUser.Email
            },registerNewUser.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" , ", result.Errors.Select(x => x.Description));
                throw new Exception($"Error : { errors }");
            }
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}