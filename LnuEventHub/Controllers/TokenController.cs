
using DAL.Moldels;
using Domain.Service;
using Domain.Service.Generic;
using Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LnuEventHub.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private readonly UserPassServiceAsync<UserPassViewModel, User> _userService;


        public TokenController(IConfiguration config, UserPassServiceAsync<UserPassViewModel, User> userService)
        {
            _config = config;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> CreateAsync([FromBody] LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = await _userService.AuthenticateAsync(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(UserPassViewModel user)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.FirstName + " "+user.LastName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            //attach roles
            foreach (string role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               _config["Jwt:Issuer"],
               _config["Jwt:Issuer"],
               claims,
              expires: DateTime.Now.AddMinutes(60),  //60 min expiry and a client monitor token quality and should request new token with this one expiries
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Authenticates login information, retrieves authorization infomation (roles)




    }
}
