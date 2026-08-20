using System.Net;

public class RouteTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _fixture;
    public RouteTests(CustomWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }
    public static IEnumerable<object[]> GetAddBookingData()
    {
        BookingDTO bookingDTO = new()
        {
            Status = BookingStatus.Draft,
            CollectionDate = new DateTime(2026, 12, 20),
            Location = new LocationDTO { MapsId = "test", Address = "test_address", Postcode = "test", Latitude = -36.11m, Longitude = 21.44m },
            RecyclingItems = new()
            {
                new() {MaterialType=MaterialTypes.aluminum, ItemCount = 3},
                new() {MaterialType=MaterialTypes.glass, ItemCount=2},
                new() {MaterialType=MaterialTypes.glass, ItemCount=2},
            }
        };
        BookingDTO bookingDTO1 = new()
        {
            Status = BookingStatus.Scheduled,
            CollectionDate = new DateTime(2026, 12, 20),
            Location = new LocationDTO { MapsId = "test", Address = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            RecyclingItems = new()
            {
                new() {MaterialType=MaterialTypes.aluminum, ItemCount = 3},
                new() {MaterialType=MaterialTypes.glass, ItemCount=2},
                new() {MaterialType=MaterialTypes.glass, ItemCount=2},
            }
        };
        yield return new object[] { bookingDTO, HttpStatusCode.Created, 1 };
        yield return new object[] { bookingDTO1, HttpStatusCode.BadRequest }; // Latitude and Longitude in Location should fail validation
    }

    [Theory]
    [MemberData(nameof(GetAddBookingData))]
    public async Task AddBookingRoute(BookingDTO booking, HttpStatusCode httpStatusCode, int? expectedValue = null)
    {
        // Arrange
        HttpClient client = _fixture.CreateClient();
        // Act
        var result = await client.PostAsync("/booking", JsonContent.Create(booking));
        // Assert
        Assert.Equal(httpStatusCode, result.StatusCode);
        if (expectedValue != null)
        {
            var resultValue = await result.Content.ReadFromJsonAsync<int>();
            Assert.Equal(expectedValue, resultValue);
        }
    }
}