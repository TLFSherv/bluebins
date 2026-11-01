using System.Net;
using FluentAssertions;

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