using Microsoft.AspNetCore.Http;

public class StubBookingService : IBookingService
{
    public Task<int?> AddBooking(string userId, BookingDTO booking)
    {
        int? bookingId = 1;
        return Task.Run(() => bookingId);
    }

    public Task<BookingView?> GetBooking(string userId, int bookingId)
    {
        throw new NotImplementedException();
    }
}