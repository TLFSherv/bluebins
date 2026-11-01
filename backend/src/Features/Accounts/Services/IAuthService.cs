public interface IAuthService
{
    Task<bool> RegisterUser(RegisterRequest request);
}

