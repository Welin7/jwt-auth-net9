using JwtAuthNet9.Dtos;
using JwtAuthNet9.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        
        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] UserDto userDto)
        {
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, userDto.Password);
            
            user.UserName = userDto.UserName;
            user.PasswordHash = hashedPassword;
            
            return Ok(user);
        }
    }
}
