using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLearn.DTOs;
using SmartLearn.Services;

namespace SmartLearn.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            if(ModelState.IsValid)
            {
                var userDomain= new SmartLearn.Models.User
                {
                    Name = requestDto.Name,
                    Email = requestDto.Email,
                    Role = requestDto.Role
                };
                var result = await _authService.RegisterAsync(userDomain, requestDto.Password);
                return result != null ? Ok(new { Token = result }) : BadRequest("Registration failed.");
            }
            return BadRequest("Invalid Request. Try again.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
        {
            if(ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(requestDto.Email, requestDto.Password);
                return result != null ? Ok(new { Token = result }) : Unauthorized("Invalid email or password.");
            }
            return BadRequest("Invalid Request. Try again.");
        }
    }
}
