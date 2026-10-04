public interface IAuthService
{
    Task<RegisterResponse> RegisterUser(RegisterRequest request);
}

