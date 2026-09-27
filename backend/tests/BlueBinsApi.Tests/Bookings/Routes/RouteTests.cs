using System.Net;
using FluentAssertions;

public class RouteTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _fixture;
    public RouteTests(CustomWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }
    public static IEnumerable<object[]> GetAddBookingData()
    {
        var recyclingItemRequests = new List<RecyclingItemRequest>()
        {
            new() {MaterialType=MaterialTypes.aluminium, Quantity = 3},
            new() {MaterialType=MaterialTypes.glass, Quantity=2},
            new() {MaterialType=MaterialTypes.glass, Quantity=2},
        };
        BookingRequest request1 = new()
        {
            UserProfile = new() { Id = "123456" },
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test", Latitude = -36.11m, Longitude = 21.44m },
            Schedule = new() { StartDate = new DateOnly(2026, 10, 2), MakeDefault = false },
            Recycling = new() { NumberOfBags = 2, RecyclingItems = recyclingItemRequests }
        };
        BookingRequest request2 = new()
        {
            UserProfile = new() { Id = "123456" },
            Schedule = new() { StartDate = new DateOnly(2026, 10, 2) },
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parsih", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            Recycling = new() { NumberOfBags = 2, RecyclingItems = recyclingItemRequests }
        };
        yield return new object[] { request1, HttpStatusCode.Created, 1 };
        yield return new object[] { request2, HttpStatusCode.BadRequest }; // Latitude and Longitude in Location should fail validation
    }

    [Theory]
    [MemberData(nameof(GetAddBookingData))]
    public async Task AddBookingRoute(BookingRequest req, HttpStatusCode httpStatusCode, int? expectedValue = null)
    {
        // Arrange
        HttpClient client = _fixture.CreateClient();
        // Act
        var result = await client.PostAsync("/booking", JsonContent.Create(req));
        // Assert
        Assert.Equal(httpStatusCode, result.StatusCode);
        if (expectedValue != null)
        {
            var resultValue = await result.Content.ReadFromJsonAsync<int>();
            Assert.Equal(expectedValue, resultValue);
        }
    }

    public static IEnumerable<object?[]> GetBookingRouteData()
    {
        List<RecyclingItemView> recyclingItems = new()
        {
            new() {MaterialType=MaterialTypes.aluminium, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
            new() {MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
            new() {MaterialType=MaterialTypes.glass, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},
        };
        var booking = new BookingView()
        {
            Status = BookingStatus.Scheduled,
            Schedule = new() { StartDate = new DateOnly(2026, 8, 20) },
            DateCreated = DateTime.Today,
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            Recycling = new() { BookingId = 1, NumberOfBags = 2, RecyclingItems = recyclingItems }
        };
        yield return new object[] { 1, booking };
        yield return new object?[] { 2, null }; // invalid booking id, should return bad request status code
    }

    [Theory]
    [MemberData(nameof(GetBookingRouteData))]
    public async Task GetBookingRoute(int bookingId, BookingView? expectedResult)
    {
        // Arrange
        HttpClient client = _fixture.CreateClient();
        // Act
        var result = await client.GetAsync($"/booking/{bookingId}");
        // Assert
        if (expectedResult is null)
        {
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
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