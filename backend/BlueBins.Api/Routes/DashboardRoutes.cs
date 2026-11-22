using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class DashboardRoutes
{
    public static IApplicationBuilder AddDashboardRoutes(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            var dashboardApi = endpoints.MapGroup("/api/dashboard");

            dashboardApi.MapGet("/bookingDates", async ([FromServices] IDashboardRepository repository, HttpContext context) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Results.Unauthorized();
                }
                var result = await repository.GetBookingDates(userId);
                return Results.Ok(result);
            }).WithName("GetBookingDates");
        });
    }
}