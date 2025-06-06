﻿using JwtAuthNet9.Dtos;
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
        public async Task<ActionResult<User>> Register([FromBody] UserDto userDto)
        {
            var user = await authService.RegisterAsync(userDto);
            if (user is null)
            {
                return BadRequest("User already exists");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] UserDto userDto)
        {
            var resultResponse = await authService.LoginAsync(userDto);
            if (resultResponse is null)
            {
                return BadRequest("Invalid username or password");
            }

            await authService.WriteAuthTokenAsHttpOnlyCookie(resultResponse.AccessToken, resultResponse.RefreshToken);

            return Ok(resultResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var result = await authService.RefreshTokenAsync(refreshTokenRequestDto);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token");
            }

            await authService.WriteAuthTokenAsHttpOnlyCookie(result.AccessToken, result.RefreshToken);

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await Task.CompletedTask;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            Response.Cookies.Append("access_token", "", cookieOptions);
            Response.Cookies.Append("refresh_token", "", cookieOptions);

            return Ok(new { message = "Logout successful" });
        }

        // This endpoint is protected and requires authentication
        [Authorize]
        [HttpGet("test-only-endpoint-authorize")]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test-only-endpoint-role-admin")]
        public IActionResult AuthenticatedOnlyRoleAdmin()
        {
            return Ok("You are the admin!");
        }
    }
}
