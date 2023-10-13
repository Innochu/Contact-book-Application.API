using ContactBook.Core.Services.Interfaces;
using ContactBook.Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO model, string role)
        {
            var registerResult = await _authService.RegisterUserAsync(model, ModelState, role);

            if (!registerResult)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(new
                {
                    Message = "User registration successful"
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.LoginAsync(model);
            if (token == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Credentials"
                });
            }
            return Ok(new
            {
                Token = token
            });

        }
    } 
}
