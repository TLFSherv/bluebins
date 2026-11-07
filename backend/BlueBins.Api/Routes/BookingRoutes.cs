using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

public static class BookingRoutes
{
    public static IApplicationBuilder AddBookingRoutes(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            var bookingApi = endpoints.MapGroup("/api/booking")
            .RequireAuthorization()
            .AddEndpointFilterFactory(BookingFilters.ValidateFactory)
            .AddEndpointFilterFactory(BookingFilters.LoggingFactory);

            // Get users active booking or booking template
            bookingApi.MapGet("/", async (HttpContext context, [FromServices] IBookingRepository repository) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var result = await repository.GetBooking(userId);

                if (result is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }).WithName("GetBooking");

            bookingApi.MapGet("/{id}", async (HttpContext context, [FromRoute] int id, [FromServices] IBookingRepository repository) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var result = await repository.GetBookingById(userId, id);

                if (result is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }).WithName("GetBookingById");

            // Create a new booking
            bookingApi.MapPost("/", async ([FromBody] BookingRequest request, HttpContext context, [FromServices] IBookingRepository repository) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var result = await repository.CreateOrUpdateBooking(userId, request);

                if (result == 0)
                {
                    return Results.NotFound();
                }
                return Results.CreatedAtRoute("GetBookingById", routeValues: new { id = result }, value: result);
            }).WithName("CreateOrUpdateBooking");

        });

    }
}