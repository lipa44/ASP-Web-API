using Microsoft.Net.Http.Headers;

namespace ReportsWebApi.Middlewares;

public class CustomCachingMiddleware
{
    private readonly RequestDelegate _next;

    public CustomCachingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.GetTypedHeaders().CacheControl =
            new CacheControlHeaderValue
            {
                Private = true,
                MaxAge = TimeSpan.FromSeconds(15),
            };

        context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };

        await _next(context);
    }
}