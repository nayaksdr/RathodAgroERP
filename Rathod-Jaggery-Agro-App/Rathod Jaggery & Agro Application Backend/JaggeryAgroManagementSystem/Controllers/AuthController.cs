using JaggeryAgroManagementSystem.DTOs;
using JaggeryAgroManagementSystem.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgroManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto) =>
            Ok(await _authService.Register(dto));

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto) =>
            Ok(await _authService.Login(dto));
    }

}
