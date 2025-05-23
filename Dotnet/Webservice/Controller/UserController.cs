using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Dto.Request;
using Service.Interface;

namespace Webservice
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.userService = userService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserRequest dto)
        {
            return Ok(await userService.Create(dto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            return Ok(await userService.Login(dto));
        }
    }
}