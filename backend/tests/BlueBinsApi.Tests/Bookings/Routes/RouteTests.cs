using System.Collections;
using System.Net;
using System.Runtime.CompilerServices;
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
        BookingRequest request1 = new()
        {
            Status = BookingStatus.Draft,
            CollectionDate = new DateTime(2026, 12, 20),
            Location = new LocationRequest { MapsId = "test", AddressLine1 = "test_address", Postcode = "test", Latitude = -36.11m, Longitude = 21.44m },
            RecyclingItems = new List<RecyclingItemRequest>()
            {
                new() {MaterialType=MaterialTypes.aluminium, MaterialCount = 3},
                new() {MaterialType=MaterialTypes.glass, MaterialCount=2},
                new() {MaterialType=MaterialTypes.glass, MaterialCount=2},
            }
        };
        BookingRequest request2 = new()
        {
            Status = BookingStatus.Scheduled,
            CollectionDate = new DateTime(2026, 12, 20),
            Location = new LocationRequest { MapsId = "test", AddressLine1 = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            RecyclingItems = new List<RecyclingItemRequest>()
            {
                new() {MaterialType=MaterialTypes.aluminium, MaterialCount = 3},
                new() {MaterialType=MaterialTypes.glass, MaterialCount=2},
                new() {MaterialType=MaterialTypes.glass, MaterialCount=2},
            }
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
            CollectionDate = new DateTime(2026, 8, 20),
            DateCreated = DateTime.Today,
            Location = new() { MapsId = "test", AddressLine1 = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            RecyclingItems = recyclingItems
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