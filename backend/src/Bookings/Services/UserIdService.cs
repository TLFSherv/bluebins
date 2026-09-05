using System.Security.Claims;

public class UserIdService : IUserIdService
{
    private readonly IHttpContextAccessor _httpAccessor;
    public UserIdService(IHttpContextAccessor httpAccessor)
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