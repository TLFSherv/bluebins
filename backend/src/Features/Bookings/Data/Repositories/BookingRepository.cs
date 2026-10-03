using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : Repository, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    { }
    // Sends users default booking information, and their existing booking if it exists
    public async Task<UserBookingView> GetUserBooking(string userId)
    {
        var user = await _context.UserProfiles
        .Include(x => x.DefaultLocation)
        .Include(x => x.DefaultSchedule)
        .Include(x => x.Bookings)
        .SingleOrDefaultAsync(x => x.Id == userId);

        UserBookingView userBooking = _mapper.Map<UserBookingView>(user);
        var mostRecentBooking = user?.MostRecentBooking;
        // Send the users active booking if the booking is active
        if (mostRecentBooking != null &&
         mostRecentBooking?.Status == BookingStatus.Scheduled)
        {
            userBooking.ExistingBooking = _mapper.Map<BookingView>(mostRecentBooking);
        }
        return userBooking;
    }
    // Creates a new database entry and returns Id
    public async Task<int> Add(string userId, BookingRequest bookingRequest)
    {
        UserProfile? user = await _context.UserProfiles
        .Include(x => x.DefaultLocation)
        .Include(x => x.DefaultSchedule)
        .FirstOrDefaultAsync(x => x.Id == userId);

        var location = _mapper.Map<Location>(bookingRequest.Location);
        var schedule = _mapper.Map<Schedule>(bookingRequest.Schedule);
        var recycling = _mapper.Map<Recycling>(bookingRequest.Recycling);

        var newBooking = new Booking(user, recycling, location, schedule);
        _context.Add(newBooking);
        // Adjust UserProfile based on new booking
        if (bookingRequest.Schedule.MakeDefault)
            user.DefaultLocation = location;
        if (bookingRequest.Location.MakeDefault)
            user.DefaultSchedule = schedule;

        await _context.SaveChangesAsync();
        return newBooking.Id;
    }

    public async Task<int> Update(BookingRequest bookingRequest)
    {
        var booking = await _context.Bookings
        .FirstOrDefaultAsync(x => x.Id == bookingRequest.Id);

        var location = _mapper.Map<Location>(bookingRequest.Location);
        var schedule = _mapper.Map<Schedule>(bookingRequest.Schedule);
        var recycling = _mapper.Map<Recycling>(bookingRequest.Recycling);

        booking.SetLocation(location, schedule);
        booking.Recycling = recycling;

        return await _context.SaveChangesAsync();
    }
}