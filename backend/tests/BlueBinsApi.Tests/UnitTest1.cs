namespace BlueBinsApi.Tests;

public class UnitTest1
{
    [Fact]
    public void ConvertsBookingDTOToBookingRequest_Successfully()
    {
        // Arrange
        string userId = "testUserId";
        BookingDTO bookingDTO = new()
        {
            ScheduleId = 1,
            Status = BookingStatus.Scheduled,
            CollectionDate = new DateTime(2026, 6, 29),
            Location = new LocationDTO { LocationId = 1, MapsId = "test", Address = "test", Postcode = "test", Latitude = 0.1M, Longitude = 0.1M },
            RecyclingItems = new List<RecyclingItemDTO> { }
        };

        // Act
        var request = BookingHelpers.ConvertBookingDtoToRequest(userId, bookingDTO);

        // Assert
        Assert.IsType<AddBookingRequest>(request);
    }
}
