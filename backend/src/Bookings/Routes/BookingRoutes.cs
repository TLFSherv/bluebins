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

            bookingApi.MapPost("/", async ([FromBody] BookingRequest request, HttpContext context, [FromServices] IBookingRepository repository, [FromServices] LinkGenerator linker) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                // use method extension to calculate and set weight and volume of materials
                request.RecyclingItems.CalculateWeightAndVolume();
                var result = await repository.Add<BookingRequest, Booking, int>(request);

                if (result == 0)
                {
                    return Results.BadRequest();
                }
                return Results.CreatedAtRoute("GetBooking", routeValues: new { id = result }, value: result);
            }).WithName("AddBooking");

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
            }).WithName("GetBooking");
        });
    }
}