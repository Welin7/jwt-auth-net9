namespace JwtAuthNet9.MiddlewareValidateToken
{
    public class JwtFromCookieMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("access_token", out var token))
            {
                context.Request.Headers.Authorization = $"Bearer {token}";
            }

            await next(context);
        }
    }
}
