public class BookingRepositoryStub : IBookingRepository
{
    public Task<int> AddBooking(string userId, BookingRequest bookingRequest)
    {
        return Task.FromResult(1);
    }

    public Task<BookingView?> GetBooking(string userId)
    {
        List<RecyclingItemView> recyclingItems = new()
        {
            new() {MaterialType=MaterialTypes.aluminium, Quantity = 5},
            new() {MaterialType=MaterialTypes.glass, Quantity = 2},
            new() {MaterialType=MaterialTypes.glass, Quantity = 7},
        };
        var booking = new BookingView()
        {
            Status = BookingStatus.Scheduled,
            Schedule = new() { Date = new DateOnly(2026, 8, 20) },
            DateCreated = DateTime.Today,
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            Recycling = new() { BookingId = 1, NumberOfBags = 2, RecyclingItems = recyclingItems }
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

    public Task<int> UpdateBooking(string userId, BookingRequest bookingRequest)
    {
        throw new NotImplementedException();
    }
}