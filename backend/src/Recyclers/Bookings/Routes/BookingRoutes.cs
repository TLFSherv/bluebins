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

            bookingApi.MapPost("/", async ([FromBody] BookingDTO bookingDTO, HttpContext context, [FromServices] IBookingService service, [FromServices] LinkGenerator linker) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var result = await service.AddBooking(userId, bookingDTO);
                if (result is null)
                {
                    Results.BadRequest();
                }
                return Results.CreatedAtRoute("GetBooking", routeValues: new { id = result }, value: result);
            }).WithName("AddBooking");

            bookingApi.MapGet("/{id:int}", async ([FromRoute] int id, HttpContext context, [FromServices] IBookingService service) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                var result = await service.GetBooking(userId, id);
                if (result is null)
                {
                    Results.BadRequest();
                }
                return Results.Ok(result);
            }).WithName("GetBooking");
        });
    }
}