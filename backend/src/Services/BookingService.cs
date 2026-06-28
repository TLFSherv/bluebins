public class BookingService
{
    private readonly IBookingRepository _repository;
    public BookingService(IBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<int?> AddBooking(string userId, BookingDTO booking)
    {
        var bookingRequest = BookingHelpers.ConvertBookingDtoToRequest(userId, booking);
        return await _repository.AddBooking(bookingRequest);
    }

    public async Task<BookingView?> GetBooking(string userId, int bookingId)
    {
        return await _repository.GetUserBooking(userId, bookingId);
    }

}
