﻿
using DAL.Moldels;
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
    [ApiController]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private readonly IServiceAsync<UserViewModel, User> _userService;

        public TokenController(IConfiguration config, IServiceAsync<UserViewModel, User> userService)
        {
            _config = config;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/")]
        public IActionResult Create([FromBody] LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Name));
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
        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            var userView = _userService.Get(x => x.Email == login.Email).Result.SingleOrDefault();
            if (userView != null && userView.Password == login.Password)
            {
                user = new UserModel { Name = userView.FirstName + " " + userView.LastName, Email = userView.Email, Roles = new string[] { } };
                foreach (string role in userView.Roles) user.Roles = new List<string>(user.Roles) { role }.ToArray();
                
            }

            return user;
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string[] Roles { get; set; }
        }
    }
}
