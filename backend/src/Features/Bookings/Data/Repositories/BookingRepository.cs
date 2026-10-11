using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : Repository, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    { }
    // Sends users default booking information, and their existing booking if it exists
    public async Task<BookingView?> GetUserBooking(string userId)
    {
        UserProfile? userProfile = await _context.UserProfiles
        .Include(x => x.DefaultLocation)
        .Include(x => x.DefaultSchedule)
        // there should only be one scheduled booking
        .Include(x => x.Bookings.Where(booking => booking.Status == BookingStatus.Scheduled))
        .SingleOrDefaultAsync(x => x.Id == userId);

        if (userProfile == null) return null;
        // Three possibilities: 
        // 1. User has an active booking and in that case just return it
        // 2. User doesn't have an active booking so return their default data
        // 3. User doesn't have default data so return empty booking
        Booking userBooking;
        if (userProfile.Bookings != null && userProfile.Bookings.Any())
        {
            userBooking = userProfile.Bookings.Single();
            return _mapper.Map<BookingView>(userBooking);
        }

        LocationView locationView = new();
        ScheduleView scheduleView = new();
        if (userProfile.DefaultLocation != null)
            locationView = _mapper.Map<LocationView>(userProfile.DefaultLocation);
        if (userProfile.DefaultSchedule != null)
            scheduleView = _mapper.Map<ScheduleView>(userProfile.DefaultSchedule);

        BookingView bookingView = new()
        {
            Location = locationView,
            Schedule = scheduleView,
            Recycling = new(),
            Status = BookingStatus.Draft
        };
        return bookingView;
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

        var newBooking = new Booking(user, recycling, location, schedule)
        {
            Status = BookingStatus.Scheduled,
            DateCreated = DateTime.Now
        };
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