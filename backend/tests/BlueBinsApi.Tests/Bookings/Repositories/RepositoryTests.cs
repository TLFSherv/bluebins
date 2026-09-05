using System.Runtime.CompilerServices;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

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
            CollectionDate = new DateTime(2026, 8, 20),
            DateCreated = DateTime.Today,
            Location = new() { MapsId = "test", AddressLine1 = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            RecyclingItems =
            [
                new() {BookingId=1, MaterialType=MaterialTypes.aluminium, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
                new() {BookingId=1, MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
                new() {BookingId=1, MaterialType=MaterialTypes.glass, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},
            ]
        };
        yield return new object[] { "123456", 1, expectedResult }; // correct userId and bookingId should return booking
        yield return new object?[] { "123456", 2, null }; // incorrect bookingId should return null
        yield return new object?[] { "badId", 1, null }; // incorrect userId should return null
    }

    [Theory]
    [MemberData(nameof(GetUserBookingTestData))]
    public async Task GetUserBooking_ReturnsCorrectBooking(string userId, int bookingId, BookingView? expectedResult)
    {
        // Arrange
        List<RecyclingItem> recyclingItems = new()
        {
            new() {Id=1, BookingId=1, MaterialType=MaterialTypes.aluminium, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
            new() {Id=2, BookingId=1, MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
            new() {Id=3, BookingId=1, MaterialType=MaterialTypes.glass, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},
        };

        context.Database.EnsureCreated();
        Booking booking = new()
        {
            Id = 1,
            UserId = "123456",
            LocationId = 1,
            Status = BookingStatus.Scheduled,
            CollectionDate = new DateTime(2026, 8, 20),
            DateCreated = DateTime.Today,
            UserProfile = new() { Id = "123456" },
            Location = new() { Id = 1, MapsId = "test", AddressLine1 = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
        };
        context.Add(booking);
        context.AddRange(recyclingItems);
        context.SaveChanges();

        var recyclingData = await context.RecyclingItems.Where(x => x.Id == 1).ToListAsync();
        var bookingData = await context.Bookings
            .Include(x => x.RecyclingItems)
            .FirstOrDefaultAsync(x => x.Id == 1);

        var repository = new BookingRepository(context, _mapper);
        // Act
        var result = await repository.GetUserBooking(userId, bookingId);
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object[]> AddUserBookingData()
    {
        LocationDTO location = new() { MapsId = "test", AddressLine1 = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 };
        List<AddRecyclingItemRequest> recyclingItems = new()
       {
            new() {MaterialType=MaterialTypes.aluminium, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
            new() {MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
            new() {MaterialType=MaterialTypes.glass, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},

       };
        ScheduleDTO schedule1 = new() { ScheduleId = 1, SetAsDefault = true, StartDate = new DateOnly(2026, 8, 8), Frequency = FrequencyTypes.Weekly, IsActive = true };
        ScheduleDTO schedule2 = new() { ScheduleId = null, SetAsDefault = false, StartDate = new DateOnly(2026, 8, 9), Frequency = FrequencyTypes.Weekly, IsActive = true };
        AddBookingRequest request1 = new() { UserId = "123456", Status = BookingStatus.Draft, Location = location, RecyclingItems = recyclingItems, Schedule = null, DateCreated = DateTime.Today };
        AddBookingRequest request2 = new() { UserId = "123456", Status = BookingStatus.Draft, Location = location, RecyclingItems = recyclingItems, Schedule = schedule1, DateCreated = DateTime.Today };
        AddBookingRequest request3 = new() { UserId = "123456", Status = BookingStatus.Draft, Location = location, RecyclingItems = recyclingItems, Schedule = schedule2, DateCreated = DateTime.Today };
        yield return new object[] { request1 }; // request with no schedule
        yield return new object[] { request2 }; // request with schedule
        yield return new object[] { request3 }; // request with new schedule
    }

    [Theory]
    [MemberData(nameof(AddUserBookingData))]
    public async Task AddUserBooking_ReturnsCorrectBookingId(AddBookingRequest request)
    {
        context.Database.EnsureCreated();
        // Arrange
        UserProfile user = new() { Id = "123456" };
        Schedule schedule = new() { Id = 1, StartDate = new DateOnly(2026, 8, 8), Frequency = FrequencyTypes.Weekly };
        context.Add(user);
        context.Add(schedule);
        context.SaveChanges();

        if (request.Schedule is not null)
        {
            Schedule newSchedule = new() { StartDate = request.Schedule.StartDate, Frequency = request.Schedule.Frequency };
        }

        var repository = new BookingRepository(context, _mapper);
        // Act
        var result = await repository.AddEntity<AddBookingRequest, Booking, int>(request);
        Assert.IsType<int>(result);
        var booking = context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.RecyclingItems)
        .FirstOrDefault(x => x.Id == result);

        Assert.IsType<Booking>(booking);
        // Assert
        Assert.Equal(request.CollectionDate, booking.CollectionDate);
        Assert.Equal(request.Location.AddressLine1, booking.Location!.AddressLine1);
        Assert.Equal(request.Schedule?.StartDate, booking.Schedule?.StartDate);
        Assert.Equal(request.RecyclingItems?.Count(), booking.RecyclingItems?.Count());

    }
}