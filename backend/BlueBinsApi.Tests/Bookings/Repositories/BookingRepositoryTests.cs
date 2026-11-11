using Application.Dtos;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Extensions;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
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

    [Theory]
    [ClassData(typeof(GetBookingByIdRepoTestData))]
    public async Task GetBookingById(int bookingId, BookingView? expectedResult)
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
        };

        Recycling recycling = new()
        {
            Id = 1,
            NumberOfBags = 2,
            RecyclingItems = new List<RecyclingItem>
                {
                    new() {RecyclingId=1, MaterialType=MaterialTypes.aluminium, Quantity = 2},
                    new() {RecyclingId=1, MaterialType=MaterialTypes.glass, Quantity = 7},
                    new() {RecyclingId=1, MaterialType=MaterialTypes.tin, Quantity = 5},
                }
        };
        Booking booking = new(location)
        {
            UserProfileId = user.Id,
            BookingDate = new DateOnly(2026, 10, 2),
            Status = BookingStatus.Scheduled,
            Recycling = recycling,
            DateCreated = DateTime.Today.AsUtc()
        };
        context.Add(user);
        context.Add(booking);
        context.SaveChanges();
        var repository = new BookingRepository(context, _mapper);
        // Act
        var result = await repository.GetBookingById(user.Id, bookingId);
        // Assert
        if (expectedResult is not null)
        {
            Assert.IsType<BookingView>(result);
        }
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [ClassData(typeof(CreateOrUpdateBookingTestData))]
    public async Task CreateOrUpdateBooking(bool isCreate, BookingRequest request)
    {
        context.Database.EnsureCreated();
        // Arrange
        UserProfile user = new() { Id = "123456" };
        context.Add(user);
        // Add booking to test update booking logic
        if (!isCreate)
        {
            Location location = new()
            {
                Id = 1,
                MapsId = "test",
                Address = "old_address",
                Parish = "old_parish",
                Postcode = "old_postcode",
            };

            Recycling recycling = new()
            {
                Id = 1,
                NumberOfBags = 2,
                RecyclingItems = new List<RecyclingItem>
                {
                    new() {RecyclingId=1, MaterialType=MaterialTypes.aluminium, Quantity = 2},
                    new() {RecyclingId=1, MaterialType=MaterialTypes.glass, Quantity = 7},
                    new() {RecyclingId=1, MaterialType=MaterialTypes.tin, Quantity = 5},
                }
            };
            Schedule schedule = new(Frequency.Biweekly, DayOfWeek.Monday, null);
            Booking updateBooking = new(location)
            {
                UserProfileId = user.Id,
                BookingDate = new DateOnly(2026, 11, 20),
                Status = BookingStatus.Scheduled,
                Schedule = schedule,
                Recycling = recycling,
                DateCreated = DateTime.Today.AsUtc()
            };
            context.Add(updateBooking);
        }
        context.SaveChanges();
        var repository = new BookingRepository(context, _mapper);

        // Act
        var result = await repository.CreateOrUpdateBooking(user.Id, request);
        Assert.IsType<int>(result);
        var booking = context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.Recycling)
        .ThenInclude(x => x.RecyclingItems)
        .FirstOrDefault(x => x.Id == result);

        Assert.IsType<Booking>(booking);
        // Assert
        Assert.Equal(request.BookingDate, booking.BookingDate);
        Assert.Equal(request.Location.Address, booking.Location!.Address);
        Assert.Equal(request.Schedule?.Frequency, booking.Schedule?.Frequency);
        Assert.Equal(request.Recycling.NumberOfBags, booking.Recycling.NumberOfBags);
    }

    [Theory]
    [ClassData(typeof(GetBookingRepoTestData))]
    public async Task GetBooking(string userId, BookingView expectedResult)
    {
        context.Database.EnsureCreated();
        // Arrange
        UserProfile user1 = new UserProfile { Id = "1" };
        Location location = new()
        {
            Id = 1,
            MapsId = "test",
            Address = "test_address",
            Parish = "test_parish",
            Postcode = "test_postcode",
        };

        Recycling recycling = new()
        {
            Id = 1,
            NumberOfBags = 2,
            RecyclingItems = new List<RecyclingItem>
                {
                    new() {MaterialType=MaterialTypes.aluminium, Quantity = 4},
                    new() {MaterialType=MaterialTypes.glass, Quantity = 5},
                    new() {MaterialType=MaterialTypes.tin, Quantity = 6},
                }
        };
        Schedule schedule = new(Frequency.Monthly, null, 14);
        Booking booking = new(location)
        {
            UserProfileId = user1.Id,
            BookingDate = new DateOnly(2026, 10, 2),
            Schedule = schedule,
            Status = BookingStatus.Scheduled,
            Recycling = recycling,
            DateCreated = DateTime.Today.AsUtc()
        };

        UserProfile user2 = new()
        {
            Id = "2",
            DefaultLocation = location,
            DefaultSchedule = schedule
        };

        UserProfile user3 = new() { Id = "3" };
        context.AddRange(new List<UserProfile> { user1, user2, user3 });
        context.Add(booking);
        context.SaveChanges();

        // Act
        // Booking repository mock
        var repository = new BookingRepository(context, _mapper);
        var result = await repository.GetBooking(userId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}