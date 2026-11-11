using Application.Dtos;

public class BookingRepositoryStub : IBookingRepository
{
    public Task<int> CreateOrUpdateBooking(string userId, BookingRequest bookingRequest)
    {
        return Task.FromResult(1);
    }

    public Task<BookingView?> GetBooking(string userId)
    {
        Dictionary<MaterialTypes, int> materialQuantities = new()
        {
            [MaterialTypes.tin] = 5,
            [MaterialTypes.aluminium] = 2,
            [MaterialTypes.glass] = 7
        };
        var booking = new BookingView()
        {
            Status = BookingStatus.Scheduled,
            BookingDate = new DateOnly(2026, 8, 20),
            Schedule = new() { Frequency = Frequency.Weekly, DayOfWeek = DayOfWeek.Wednesday },
            DateCreated = DateTime.Today,
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            Recycling = new() { BookingId = 1, NumberOfBags = 2, MaterialQuantities = materialQuantities }
        };
        return Task.FromResult<BookingView?>(booking);
    }

    public Task<BookingView?> GetBookingById(string userId, int bookingId)
    {
        if (bookingId == 1)
        {
            return GetBooking(userId);
        }
        return Task.FromResult<BookingView?>(null);
    }

}