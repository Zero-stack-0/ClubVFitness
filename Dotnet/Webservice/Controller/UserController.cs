using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Dto.Request;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.Helper;
using Model.Entities;
using Service.Dto.Response;

namespace Webservice
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IConfiguration configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest dto)
        {
            var result = await userService.Create(dto);
            if (result.StatusCode == StatusCodes.Status201Created)
            {
                var user = result.Data as UserResponse;
                var token = GenerateToken(user.Email, user.Role.Name);
                return Ok(new ApiResponse().SucessResponse(GenerateToken(user.Email, user.Role.ToString())));
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            var result = await userService.Login(dto);
            if (result.StatusCode == 200)
            {
                var user = result.Data as UserResponse;
                var token = GenerateToken(user.Email, user.Role.Name);
                return Ok(new ApiResponse().SucessResponse(GenerateToken(user.Email, user.Role.ToString())));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (username == null || role == null)
            {
                return Ok(new ApiResponse(null, 401, "Unauthorized"));
            }
            var data = await userService.GetUserProfile(username);
            return Ok(data);
        }

        public string GenerateToken(string username, string role)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])); // should be in config
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["issuer"],
                audience: jwtSettings["audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}