using System.Reflection;
using EllipticCurve;
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
           if (routeName == RouteNames.AddBooking)
           {
               var validator = context.ApplicationServices.GetRequiredService<IValidator<BookingDTO>>();
               var input = invocationContext.GetArgument<BookingDTO>(parameterPosition);
               var validationResults = await validator.ValidateAsync(input);
               if (!validationResults.IsValid)
               {
                   return Results.ValidationProblem(validationResults.ToDictionary());
               }
           }
           return next(invocationContext);
       };
    }

    internal static EndpointFilterDelegate LoggingFactory(
        EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {

        ParameterInfo[] parameters = context.MethodInfo.GetParameters();
        var (routeName, parameterPosition) = GetRouteInfo(parameters);

        if (routeName == RouteNames.Unknown)
        {
            return next;
        }

        var logger = context.ApplicationServices.GetRequiredService<ILogger>();
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
                        var input = invocationContext.GetArgument<BookingDTO>(parameterPosition);
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
            return next(invocationContext);
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
            }
            else if (parameters[i].Name == "bookingDTO" && parameters[i].ParameterType == typeof(BookingDTO))
            {
                routeName = RouteNames.AddBooking;
                parameterPosition = i;
            }
        }
        return (routeName, parameterPosition);
    }
}

