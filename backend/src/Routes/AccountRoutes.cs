using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public static class AccountRoutes
{
    public static IApplicationBuilder AddAccountRoutes(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            var accountApi = endpoints.MapGroup("/api/account");

            // add Identity endpoints to route group
            accountApi.MapIdentityApi<ApplicationUser>();

            accountApi.MapPost("/logout", async ([FromServices] SignInManager<ApplicationUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }).RequireAuthorization();

            accountApi.MapGet("/isSignedIn", (HttpContext context, [FromServices] SignInManager<ApplicationUser> signInManager) =>
            {
                // Invoke the method by passing the current user
                bool signedIn = signInManager.IsSignedIn(context.User);

                return Results.Ok(new { IsSignedIn = signedIn });
            });

            accountApi.MapGet("/login/google", ([FromQuery] string returnUrl, LinkGenerator linkGenerator,
            SignInManager<ApplicationUser> signInManager, HttpContext context) =>
            {
                var redirectUrl = linkGenerator.GetPathByName("GoogleLoginCallback", new { returnUrl });
                var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

                return Results.Challenge(properties, ["Google"]);
            });

            accountApi.MapGet("/login/google/callback", async (
                [FromQuery] string returnUrl,
                SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager,
                HttpContext context) =>
            {
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return Results.BadRequest("Error loading external login information from Google.");
                }

                var signInResult = await signInManager.ExternalLoginSignInAsync(
                    info.LoginProvider,
                    info.ProviderKey,
                    isPersistent: true,
                    bypassTwoFactor: true);

                var finalTarget = string.IsNullOrEmpty(returnUrl) ? "http://localhost:3000/" : returnUrl;

                if (signInResult.Succeeded)
                {
                    // Force an immediate response-level redirect, bypassing downstream pipeline execution
                    return Results.Redirect(finalTarget);
                }

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    return Results.BadRequest("Email claim not received from Google.");
                }

                var user = new ApplicationUser { UserName = email, Email = email };
                var createResult = await userManager.CreateAsync(user);

                if (createResult.Succeeded)
                {
                    // Link the Google account login token to the local Identity database user
                    var loginResult = await userManager.AddLoginAsync(user, info);
                    if (loginResult.Succeeded)
                    {
                        // Establish the local application cookie session
                        await signInManager.SignInAsync(user, isPersistent: true);

                        // Return clean redirect execution object
                        return Results.Redirect(finalTarget);
                    }
                }

                return Results.BadRequest("Failed to complete local account creation or association.");

            }).WithName("GoogleLoginCallback");
        });
    }
}