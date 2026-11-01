using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public BookingRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<BookingView?> GetBooking(string userId)
    {
        // Two scenarios: 
        // 1. User has an active booking and in that case just return it
        // 2. User doesn't have an active booking so return new booking with default data

        // 1. Return users active booking
        Booking? booking = await _context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.Recycling)
        .ThenInclude(x => x.RecyclingItems)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.UserProfileId == userId && x.Status == BookingStatus.Scheduled);

        if (booking != null)
        {
            return _mapper.Map<BookingView>(booking);
        }

        // 2. Return new booking with users default data
        UserProfile userProfile = await _context.UserProfiles
        .Include(x => x.DefaultLocation)
        .Include(x => x.DefaultSchedule)
        .AsNoTracking()
        .SingleAsync(x => x.Id == userId);

        return new()
        {
            Location = (userProfile.DefaultLocation != null) ?
                _mapper.Map<LocationView>(userProfile.DefaultLocation) : new(),
            Schedule = (userProfile.DefaultSchedule != null) ?
                _mapper.Map<ScheduleView>(userProfile.DefaultSchedule) : new(),
            Recycling = new(),
            Status = BookingStatus.Draft
        };
    }

    public async Task<BookingView?> GetBookingById(string userId, int bookingId)
    {
        var booking = await _context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.Recycling)
        .ThenInclude(x => x.RecyclingItems)
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.UserProfileId == userId && x.Id == bookingId);

        return (booking != null) ?
        _mapper.Map<BookingView>(booking) : null;
    }
    public async Task<int> AddBooking(string userId, BookingRequest bookingRequest)
    {
        // Check for active booking
        bool hasActiveBooking = await _context.Bookings
        .AnyAsync(x => x.UserProfileId == userId && x.Status == BookingStatus.Scheduled);

        if (hasActiveBooking)
        {
            throw new InvalidOperationException("User already has an active booking");
        }

        // Map the request
        var booking = _mapper.Map<Booking>(bookingRequest);
        booking.UserProfileId = userId;

        // Check if the location and schedule already exist
        var foundLocation = await _context.Locations
        .FirstOrDefaultAsync(x => x.Address == booking.Location.Address);

        var foundSchedule = await _context.Schedules
        .FirstOrDefaultAsync(x => x.Date == booking.Schedule.Date && x.Frequency == booking.Schedule.Frequency);

        booking.ChangeLocationAndSchedule(
            foundLocation ?? booking.Location,
            foundSchedule ?? booking.Schedule
        );

        _context.Add(booking);

        // Adjust UserProfile based on new booking
        var user = await _context.UserProfiles.FirstAsync(x => x.Id == userId);

        if (bookingRequest.Schedule.MakeDefault)
        {
            user.DefaultSchedule = booking.Schedule;
        }
        if (bookingRequest.Location.MakeDefault)
        {
            user.DefaultLocation = booking.Location;
        }
        await _context.SaveChangesAsync();
        return booking.Id;
    }

    public async Task<int> UpdateBooking(string userId, BookingRequest bookingRequest)
    {
        // Get existing booking and its relationships
        var booking = await _context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.Recycling)
        .SingleOrDefaultAsync(x => x.Id == bookingRequest.Id && x.UserProfileId == userId);

        // If there's no booking throw an error
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {bookingRequest.Id} was not found");
        }

        // Check if new location and schedule already exist, so it's not added to the db again
        var foundLocation = await _context.Locations.SingleOrDefaultAsync(x =>
         x.Address == bookingRequest.Location.Address);
        var foundSchedule = await _context.Schedules.SingleOrDefaultAsync(x =>
        x.Date == bookingRequest.Schedule.Date && x.Frequency == bookingRequest.Schedule.Frequency);

        // Map the booking request
        var bookingUpdate = _mapper.Map<Booking>(bookingRequest);

        // Use business logic in booking model to change location and schedule
        booking.ChangeLocationAndSchedule(
            foundLocation ?? bookingUpdate.Location,
            foundSchedule ?? bookingUpdate.Schedule
        );
        // Remove old recycling data from db and replace 
        booking.Recycling = bookingUpdate.Recycling;

        // Change users default preferences
        var user = await _context
        .UserProfiles
        .Include(x => x.DefaultLocation)
        .Include(x => x.DefaultSchedule)
        .SingleAsync(x => x.Id == userId);

        if (bookingRequest.Schedule.MakeDefault)
        {
            user.DefaultSchedule = booking.Schedule;
        }
        if (bookingRequest.Location.MakeDefault)
        {
            user.DefaultLocation = booking.Location;
        }

        await _context.SaveChangesAsync();
        return booking.Id;
    }
}