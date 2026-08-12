using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

public static class BookingRoutes
{
    public static IApplicationBuilder AddBookingRoutes(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            var bookingApi = endpoints.MapGroup("/booking")
            .AddEndpointFilterFactory(BookingFilters.ValidateFactory)
            .AddEndpointFilterFactory(BookingFilters.LoggingFactory);

            bookingApi.MapPost("/", async ([FromBody] BookingDTO bookingDto, HttpContext context, [FromServices] BookingService service, LinkGenerator linker) =>
            {
                try
                {
                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId is null)
                    {
                        return Results.Unauthorized();
                    }

                    var result = await service.AddBooking(userId, bookingDto);
                    if (result is null)
                    {
                        Results.BadRequest();
                    }

                    return Results.CreatedAtRoute(linker.GetPathByName("GetBooking", new { id = result }), result);
                }
                catch (Exception e)
                {
                    // return problem details
                    return Results.Problem(e.Message);
                }
            }).WithName("AddBooking");

            bookingApi.MapGet("/{id:int}", async (int id, HttpContext context, BookingService service) =>
            {
                try
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
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }

            }).WithName("GetBooking");
        });
    }
}