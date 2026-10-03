using System.Security.Claims;

public class HelperService : IHelperService
{
    private readonly IHttpContextAccessor _httpAccessor;
    public HelperService(IHttpContextAccessor httpAccessor)
    {
        _httpAccessor = httpAccessor;
    }
    public string GetUserId()
    {
        var httpContext = _httpAccessor.HttpContext;

        if (httpContext == null)
            return string.Empty;

        return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}