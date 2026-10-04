using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    public async Task<RegisterResponse> RegisterUser(RegisterRequest request)
    {
        // Use an EF Core execution strategy to handle retries cleanly with transactions
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Create Idenity User
            var identityUser = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var createResult = (request.Password is null) ?
            await _userManager.CreateAsync(identityUser) :
            await _userManager.CreateAsync(identityUser, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Identity creation failed: {errors}");
            }

            // Create UserProfile linking the generated UserId
            var userProfile = new UserProfile
            {
                Id = identityUser.Id,
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            // Commit transaction
            await transaction.CommitAsync();
            return new RegisterResponse(identityUser.Id, identityUser.Email);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}