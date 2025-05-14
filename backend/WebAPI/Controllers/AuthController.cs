using Microsoft.AspNetCore.Mvc;
using FlipCardApp.Application.Interfaces;
using FlipCardApp.Application.DTOs;
using FlipCardApp.Domain;

namespace FlipCardApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
                return BadRequest("Passwords do not match");

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _authService.Register(user, registerDto.Password);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var result = await _authService.Login(loginDto.Username, loginDto.Password);

            if (!result.Success)
                return Unauthorized(result.Token);

            return Ok(new { token = result.Token });
        }
    }
}
