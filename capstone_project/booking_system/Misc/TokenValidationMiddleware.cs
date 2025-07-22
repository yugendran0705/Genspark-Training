namespace BookingSystem.Misc;
using BookingSystem.Interfaces;
public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITokenCacheService tokenCache)
    {
        // var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        // if (!string.IsNullOrEmpty(token) && !tokenCache.IsTokenValid(token))
        // {
        //     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //     await context.Response.WriteAsync("Token is invalidated.");
        //     return;
        // }

        await _next(context);
    }
}
