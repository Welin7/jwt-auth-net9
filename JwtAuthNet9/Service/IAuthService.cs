using JwtAuthNet9.Dtos;
using JwtAuthNet9.Entities;

namespace JwtAuthNet9.Service
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto userDto);
        Task<TokenResponseDto?> LoginAsync(UserDto userDto);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto);
        Task WriteAuthTokenAsHttpOnlyCookie(string accessToken, string refreshToken);
    }
}
