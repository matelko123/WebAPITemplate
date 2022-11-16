using System.Diagnostics;

namespace WebAPITemplate.API.Middlewares;

public class RequestTimeMiddleware : IMiddleware
{
    private readonly Stopwatch _stopWatch;

    public RequestTimeMiddleware()
    {
        _stopWatch = new Stopwatch();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _stopWatch.Start();
        await next.Invoke(context);
        _stopWatch.Stop();

        long elapsedTime = _stopWatch.ElapsedMilliseconds;
        if (elapsedTime / 1000 > 4)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong.");
        }
    }
}