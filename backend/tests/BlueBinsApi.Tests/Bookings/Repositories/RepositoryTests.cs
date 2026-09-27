using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

public class RepositoryTests : IntegrationTestBase
{
    private readonly IMapper _mapper;
    public RepositoryTests(WebApplicationFactory<Program> fixture) : base(fixture)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BookingProfile>();
        }, NullLoggerFactory.Instance);

        _mapper = config.CreateMapper();

    }

    // Method providing test data
    public static IEnumerable<object?[]> GetUserBookingTestData()
    {
        BookingView expectedResult = new()
        {

            Status = BookingStatus.Scheduled,
            Schedule = new() { StartDate = new DateOnly(2026, 10, 2), IsDefault = false },
            DateCreated = DateTime.Today,
            Location = new()
            {
                MapsId = "test",
                Address = "test_address",
                Parish = "test_parish",
                Postcode = "test_postcode",
                Latitude = 0,
                Longitude = 0
            },
            Recycling = new()
            {
                BookingId = 1,
                NumberOfBags = 2,
                RecyclingItems = new List<RecyclingItemView>
                {
                    new() {MaterialType=MaterialTypes.aluminium, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
                    new() {MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
                    new() {MaterialType=MaterialTypes.tin, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},
                }
            }
        };
        yield return new object[] { 1, expectedResult }; // correct bookingId should return booking
        yield return new object?[] { 2, null }; // incorrect bookingId should return null
        yield return new object?[] { 3, null }; // incorrect bookingId should return null
    }

    [Theory]
    [MemberData(nameof(GetUserBookingTestData))]
    // This tests that the repository method GetUserBooking returns the booking successfully
    public async Task GetUserBooking_ReturnsCorrectBooking(int bookingId, BookingView? expectedResult)
    {
        // Arrange
        context.Database.EnsureCreated();
        UserProfile user = new UserProfile { Id = "123456" };
        Location location = new()
        {
            Id = 1,
            MapsId = "test",
            Address = "test_address",
            Parish = "test_parish",
            Postcode = "test_postcode",
            Latitude = 0,
            Longitude = 0
        };

        Recycling recycling = new()
        {
            Id = 1,
            NumberOfBags = 2,
            RecyclingItems = new List<RecyclingItem>
                {
                    new() {RecyclingId=1, MaterialType=MaterialTypes.aluminium, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
                    new() {RecyclingId=1, MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
                    new() {RecyclingId=1, MaterialType=MaterialTypes.tin, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},
                }
        };
        Schedule schedule = new() { StartDate = new DateOnly(2026, 10, 2) };
        Booking booking = new(user, recycling, location, schedule);

        context.Add(booking);
        context.SaveChanges();
        var test = booking;
        var repository = new BookingRepository(context, _mapper);
        // Act
        var result = await repository.Get<int, Booking, BookingView>(bookingId);
        // Assert
        if (expectedResult is not null)
        {
            Assert.IsType<BookingView>(result);
        }
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object[]> AddUserBookingData()
    {
        UserProfile user = new UserProfile { Id = "123456" };
        LocationRequest location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 };
        RecyclingRequest recycling = new()
        {
            Id = 1,
            NumberOfBags = 2,
            RecyclingItems = new()
            {
                new() {MaterialType=MaterialTypes.aluminium, Quantity = 6},
                new() {MaterialType=MaterialTypes.glass, Quantity = 3},
                new() {MaterialType=MaterialTypes.tin, Quantity = 8},
            }
        };


        ScheduleRequest schedule1 = new() { Id = 1, StartDate = new DateOnly(2026, 10, 1), MakeDefault = true, Frequency = Frequency.Weekly };
        ScheduleRequest schedule2 = new() { Id = 1, StartDate = new DateOnly(2026, 10, 12), MakeDefault = false, Frequency = Frequency.Triweekly };
        BookingRequest request1 = new() { UserProfile = user, Location = location, Recycling = recycling, Schedule = schedule1 };
        BookingRequest request2 = new() { UserProfile = user, Location = location, Recycling = recycling, Schedule = schedule1 };
        BookingRequest request3 = new() { UserProfile = user, Location = location, Recycling = recycling, Schedule = schedule2 };
        yield return new object[] { request1 }; // request with no schedule
        yield return new object[] { request2 }; // request with existing schedule
        yield return new object[] { request3 }; // request with new schedule
    }

    [Theory]
    [MemberData(nameof(AddUserBookingData))]
    //  This tests that the AddUserBooking method successfully adds bookings
    public async Task AddUserBooking_ReturnsCorrectBookingId(BookingRequest request)
    {
        context.Database.EnsureCreated();
        // Arrange
        UserProfile user = new() { Id = "123456" };
        context.Add(user);
        context.SaveChanges();

        var repository = new BookingRepository(context, _mapper);
        // Act
        var result = await repository.Add(request);
        Assert.IsType<int>(result);
        var booking = context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.Recycling)
        .ThenInclude(x => x.RecyclingItems)
        .FirstOrDefault(x => x.Id == result);

        Assert.IsType<Booking>(booking);
        // Assert
        Assert.Equal(request.Schedule.StartDate, booking.Schedule.StartDate);
        Assert.Equal(request.Location.Address, booking.Location!.Address);
        Assert.Equal(request.Schedule?.Frequency, booking.Schedule?.Frequency);
        Assert.Equal(request.Recycling?.RecyclingItems?.Count(), booking.Recycling.RecyclingItems?.Count());

    }
}