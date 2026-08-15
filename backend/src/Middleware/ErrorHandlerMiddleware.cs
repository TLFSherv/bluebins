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
            // log the error
            var traceId = Guid.NewGuid();
            logger.LogError($"Error occured while processing the request, TraceId: ${traceId}," +
            $"Meesage:${ex.Message},StackTrace:${ex.StackTrace}");

            var response = context.Response;
            response.ContentType = "application/json";

            // return the response code and message
            var (status, message) = GetResponse(ex);
            response.StatusCode = (int)status;
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