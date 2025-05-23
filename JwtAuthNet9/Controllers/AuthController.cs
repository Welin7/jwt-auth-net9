using JwtAuthNet9.Dtos;
using JwtAuthNet9.Entities;
using JwtAuthNet9.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {        
        [HttpPost("register")]
        public async Task <ActionResult<User>> Register([FromBody] UserDto userDto)
        {            
            var user = await authService.RegisterAsync(userDto);
            if (user is null)
            {
                return BadRequest("User already exists");
            }
            
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task <ActionResult<string>> Login([FromBody] UserDto userDto)
        {
            var token = await authService.LoginAsync(userDto);
            if (token is null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(token);
        }

        // This endpoint is protected and requires authentication
        [Authorize]
        [HttpGet("TestOnlyEndpoint")]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }
    }
}
