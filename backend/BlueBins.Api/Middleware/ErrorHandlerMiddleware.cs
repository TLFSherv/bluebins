using System.Net;
using Newtonsoft.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, ILogger<ErrorHandlerMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // 1. Ensure response hasn't already started writing headers
            if (context.Response.HasStarted)
            {
                logger.LogWarning("The response has already started, error handler middleware cannot write custom error payload.");
                throw;
            }

            // 2. Safe structured logging (removes manual $ characters)
            var traceId = Guid.NewGuid();
            logger.LogError(ex, "Error occurred while processing request. TraceId: {TraceId}", traceId);

            var response = context.Response;

            // 3. Set status code BEFORE setting content-type or writing to body
            var (status, message) = GetResponse(ex);
            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            await response.WriteAsync(message);
        }
    }
    private (HttpStatusCode code, string message) GetResponse(Exception ex)
    {
        HttpStatusCode code;
        switch (ex)
        {
            case KeyNotFoundException
                or FileNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                break;
            case ArgumentException
                or InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }
        return (code, JsonConvert.SerializeObject(new { ErrorMessage = ex.Message }));
    }
}