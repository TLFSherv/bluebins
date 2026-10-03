using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

public static class BookingRoutes
{
    public static IApplicationBuilder AddBookingRoutes(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            var bookingApi = endpoints.MapGroup("/booking")
            .AddEndpointFilterFactory(BookingFilters.LoggingFactory)
            .AddEndpointFilterFactory(BookingFilters.ValidateFactory);
            // Get users active booking or booking template
            bookingApi.MapGet("/", async (HttpContext context, [FromServices] IBookingRepository repository) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                var result = await repository.GetUserBooking(userId);
                if (result is null)
                {
                    return Results.BadRequest();
                }
                return Results.Ok(result);
            }).WithName("GetBooking");
            // Get a booking by booking id
            bookingApi.MapGet("/{id:int}", async ([FromRoute] int id, HttpContext context, [FromServices] IBookingRepository repository) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                var result = await repository.Get<int, Booking, BookingView>(id);
                if (result is null)
                {
                    return Results.BadRequest();
                }
                return Results.Ok(result);
            }).WithName("GetBookingById");
            // Create a new booking
            bookingApi.MapPost("/", async ([FromBody] CreateBookingRequest request, HttpContext context, [FromServices] IBookingService service) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var result = await service.AddUserBooking(request);

                if (result == 0)
                {
                    return Results.BadRequest();
                }
                return Results.CreatedAtRoute("GetBookingById", routeValues: new { id = result }, value: result);
            }).WithName("AddBooking");
        });
    }
}