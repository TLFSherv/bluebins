using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

public class RouteTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _fixture;
    public RouteTests(CustomWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(AddBookingRouteTestData))]
    public async Task AddBookingRoute(BookingRequest req, HttpStatusCode httpStatusCode, int? expectedValue = null)
    {
        // Arrange
        HttpClient client = _fixture.CreateClient();
        // Act
        var result = await client.PostAsync("/api/booking", JsonContent.Create(req));
        // Assert
        Assert.Equal(httpStatusCode, result.StatusCode);
        if (expectedValue != null)
        {
            var resultValue = await result.Content.ReadFromJsonAsync<int>();
            Assert.Equal(expectedValue, resultValue);
        }
        else
        {
            // Check the validation results
            var problemDetails = await result.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problemDetails);
            Assert.Contains("Location.Latitude", problemDetails.Errors.Keys);
            Assert.Contains("Location.Longitude", problemDetails.Errors.Keys);
            Assert.Contains("'Location Latitude' must not be equal to '0'.", problemDetails.Errors["Location.Latitude"]);
            Assert.Contains("'Location Longitude' must not be equal to '0'.", problemDetails.Errors["Location.Longitude"]);
        }
    }

    [Theory]
    [ClassData(typeof(GetBookingRouteTestData))]
    public async Task GetBookingRoute(int bookingId, BookingView? expectedResult)
    {
        // Arrange
        HttpClient client = _fixture.CreateClient();
        // Act
        var result = await client.GetAsync($"/api/booking/{bookingId}");
        // Assert
        if (expectedResult is null)
        {
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        else
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var resultValue = await result.Content.ReadFromJsonAsync<BookingView>();
            Assert.IsType<BookingView>(resultValue);
            resultValue.Should().BeEquivalentTo(expectedResult);
        }
    }
}