using System.Reflection;
using FluentValidation;

public enum RouteNames
{
    GetBooking = 1,
    AddBooking = 2,
    UpdateBooking = 3,
    DeleteBooking = 4,
    Unknown = 5
};

public class BookingFilters
{
    internal static EndpointFilterDelegate ValidateFactory(
        EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {
        ParameterInfo[] parameters = context.MethodInfo.GetParameters();
        var (routeName, parameterPosition) = GetRouteInfo(parameters);

        if (routeName == RouteNames.Unknown)
        {
            return next;
        }

        return async (invocationContext) =>
       {
           // add validation filter for the add booking route
           if (routeName == RouteNames.AddBooking)
           {
               // get validator from DI container
               var validator = context.ApplicationServices.GetRequiredService<IValidator<AddBookingRequest>>();
               // get route input parameters, specifically the booking DTO
               var input = invocationContext.GetArgument<AddBookingRequest>(parameterPosition);
               var validationResults = await validator.ValidateAsync(input);
               if (!validationResults.IsValid)
               {
                   return Results.ValidationProblem(validationResults.ToDictionary());
               }
           }
           return await next(invocationContext);
       };
    }

    internal static EndpointFilterDelegate LoggingFactory(
        EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {

        ParameterInfo[] parameters = context.MethodInfo.GetParameters();
        var (routeName, parameterPosition) = GetRouteInfo(parameters);
        // if route can't be found continue to next filter in pipeline
        if (routeName == RouteNames.Unknown)
        {
            return next;
        }
        // add logging filter
        var logger = context.ApplicationServices.GetRequiredService<ILogger<BookingFilters>>();
        return async (invocationContext) =>
        {
            switch (routeName)
            {
                case RouteNames.GetBooking:
                    {
                        var input = invocationContext.GetArgument<int>(parameterPosition);
                        logger.LogInformation("Fetching booking with id {Id}", input);
                        object? result = await next(invocationContext);
                        if (result is null)
                        {
                            logger.LogError("Failed to fetch booking with id {id}", input);
                            return next;
                        }
                        logger.LogInformation("Successfully fetched booking with id {id}", input);
                        return result;
                    }
                case RouteNames.AddBooking:
                    {
                        var input = invocationContext.GetArgument<AddBookingRequest>(parameterPosition);
                        logger.LogInformation("Adding booking new booking");
                        object? result = await next(invocationContext);
                        if (result is null)
                        {
                            logger.LogError("Failed to add booking");
                            return next;
                        }

                        logger.LogInformation("Successfully added new booking with id {id}", result);
                        return result;
                    }
            }
            return await next(invocationContext);
        };
    }

    internal static (RouteNames RouteName, int ParameterPosition) GetRouteInfo(ParameterInfo[] parameters)
    {
        RouteNames routeName = RouteNames.Unknown;
        int parameterPosition = 0;
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].Name == "id" && parameters[i].ParameterType == typeof(int))
            {
                routeName = RouteNames.GetBooking;
                parameterPosition = i;
                break;
            }
            else if (parameters[i].ParameterType == typeof(AddBookingRequest))
            {
                routeName = RouteNames.AddBooking;
                parameterPosition = i;
                break;
            }
        }
        return (routeName, parameterPosition);
    }
}

