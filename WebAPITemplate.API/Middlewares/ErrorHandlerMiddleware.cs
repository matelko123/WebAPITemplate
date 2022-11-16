using System.Net;
using Newtonsoft.Json;
using WebAPITemplate.API.Exceptions;

namespace WebAPITemplate.API.Middlewares;

public class ErrorHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex) // 400
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (NotFoundException ex) // 404
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private Task HandleExceptionAsync(
        HttpContext context, 
        Exception exception, 
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        string result;
#if DEBUG
        result = JsonConvert.SerializeObject(new { error = exception.Message });
#else
		result = JsonConvert.SerializeObject(new { error = "An error occured while processing your request." });
#endif
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
}