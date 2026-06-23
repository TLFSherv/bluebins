using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public static class BookingRoutes
{
    public static IApplicationBuilder AddBookingRoutes(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            var bookingApi = endpoints.MapGroup("/booking");

            bookingApi.MapPost("/", async (HttpContext context) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            });

        });
    }
}