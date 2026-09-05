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

            bookingApi.MapPost("/", async ([FromBody] BookingDTO bookingDTO, HttpContext context, [FromServices] IBookingRepository repository, [FromServices] LinkGenerator linker) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                AddBookingRequest addBooking = new()
                {
                    UserId = userId,
                    Schedule = bookingDTO.Schedule,
                    Status = bookingDTO.Status,
                    CollectionDate = bookingDTO.CollectionDate,
                    Location = bookingDTO.Location,
                    RecyclingItems = bookingDTO.RecyclingItems.ToRequest(),
                    DateCreated = DateTime.Now,
                };
                var result = await repository.AddEntity<AddBookingRequest, Booking, int>(addBooking);

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
                var result = await repository.GetUserBooking(userId, id);
                if (result is null)
                {
                    return Results.BadRequest();
                }
                return Results.Ok(result);
            }).WithName("GetBooking");
        });
    }
}