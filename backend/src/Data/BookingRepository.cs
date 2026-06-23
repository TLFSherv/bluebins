using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;
    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> AddBooking(AddBookingRequest request)
    {
        var booking = new Booking()
        {
            UserProfileId = request.UserProfileId,
            LocationId = request.LocationId,
            ScheduleId = request.ScheduleId,
            Status = request.Status,
            CollectionDate = request.CollectionDate,
            DateCreated = request.DateCreated,
            DateModified = request.DateModified
        };
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        return booking.Id;
    }

    public async Task<int> AddLocation(AddLocationRequest request)
    {
        var location = new Location()
        {
            MapsId = request.MapsId,
            AddressLine1 = request.Address,
            Postcode = request.Postcode,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Details = request.Details
        };
        await _context.AddAsync(location);
        await _context.SaveChangesAsync();
        return location.Id;
    }

    public async Task<int> AddRecyclingItem(AddRecyclingItemRequest request)
    {
        var recyclingItem = new RecyclingItem()
        {
            BookingId = request.BookingId,
            MaterialType = request.MaterialType,
            WeightKg = request.WeightKg,
            VolumeLiters = request.VolumeLiters,
            ContaminationPercent = request.ContaminationPercent
        };
        await _context.AddAsync(recyclingItem);
        await _context.SaveChangesAsync();
        return recyclingItem.Id;
    }

    public async Task<int> AddSchedule(AddScheduleRequest request)
    {
        var schedule = new Schedule()
        {
            StartDate = request.StartDate,
            Frequency = request.Frequency,
            IsActive = request.IsActive
        };
        await _context.AddAsync(schedule);
        await _context.SaveChangesAsync();
        return schedule.Id;
    }

    public async Task<int> AddUserProfile(AddUserProfileRequest request)
    {
        var userProfile = new UserProfile()
        {
            DefaultLocationId = request.DefaultLocationId,
            DefaultScheduleId = request.DefaultScheduleId,
            IsDeleted = request.IsDeleted,
        };
        await _context.AddAsync(userProfile);
        await _context.SaveChangesAsync();
        return userProfile.Id;
    }

    public async Task<BookingView?> GetBooking(int bookingId)
    {
        return await _context.Bookings
        .Where(x => x.Id == bookingId)
        .Select(x => new BookingView
        {
            UserProfileId = x.UserProfileId,
            LocationId = x.LocationId,
            ScheduleId = x.ScheduleId,
            Status = x.Status,
            CollectionDate = x.CollectionDate,
            DateCreated = x.DateCreated,
            DateModified = x.DateModified
        }).SingleOrDefaultAsync();
    }

    public async Task<LocationView?> GetLocation(int locationId)
    {
        return await _context.Locations
        .Where(x => x.Id == locationId)
        .Select(x => new LocationView
        {
            MapsId = x.MapsId,
            Address = x.AddressLine1,
            Postcode = x.Postcode,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            Details = x.Details
        }).SingleOrDefaultAsync();
    }

    public async Task<RecyclingItemView?> GetRecyclingItem(int recyclingItemId)
    {
        return await _context.RecyclingItems
        .Where(x => x.Id == recyclingItemId)
        .Select(x => new RecyclingItemView
        {
            MaterialType = x.MaterialType,
            WeightKg = x.WeightKg,
            VolumeLiters = x.VolumeLiters,
            ContaminationPercent = x.ContaminationPercent
        }).SingleOrDefaultAsync();
    }

    public async Task<ScheduleView?> GetSchedule(int scheduleId)
    {
        return await _context.Schedules
        .Where(x => x.Id == scheduleId)
        .Select(x => new ScheduleView
        {
            StartDate = x.StartDate,
            Frequency = x.Frequency,
            IsActive = x.IsActive
        }).SingleOrDefaultAsync();
    }

    public async Task<UserProfileView?> GetUserProfile(int userProfileId)
    {
        return await _context.UserProfiles
        .Where(x => x.Id == userProfileId)
        .Select(x => new UserProfileView
        {
            DefaultLocationId = x.DefaultLocationId,
            DefaultScheduleId = x.DefaultScheduleId,
            IsDeleted = x.IsDeleted
        }).SingleOrDefaultAsync();
    }

    public async Task<int> UpdateBooking(UpdateBookingRequest request)
    {
        var booking = await _context.Bookings.FindAsync(request.Id);

        if (booking is null)
        {
            throw new Exception("Unable to find the booking");
        }

        booking.CollectionDate = request.CollectionDate;
        booking.UserProfileId = request.UserProfileId;
        booking.LocationId = request.LocationId;
        booking.ScheduleId = request.ScheduleId;
        booking.Status = request.Status;
        booking.CollectionDate = request.CollectionDate;
        booking.DateCreated = request.DateCreated;
        booking.DateModified = request.DateModified;

        await _context.SaveChangesAsync();
        return booking.Id;
    }

    public async Task<int> UpdateLocation(UpdateLocationRequest request)
    {
        var location = await _context.Locations.FindAsync(request.LocationId);

        if (location is null)
        {
            throw new Exception("Unable to find location");
        }

        location.MapsId = request.MapsId;
        location.AddressLine1 = request.Address;
        location.Postcode = request.Postcode;
        location.Latitude = request.Latitude;
        location.Longitude = request.Longitude;
        location.Details = request.Details;

        await _context.SaveChangesAsync();
        return location.Id;
    }

    public Task<int> UpdateRecyclingItem(UpdateRecyclingItemRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateSchedule(UpdateScheduleRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateUserProfile(UpdateUserProfileRequest request)
    {
        throw new NotImplementedException();
    }
}