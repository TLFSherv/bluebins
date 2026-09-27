using Moq;

public class BookingRepositoryStub : IBookingRepository
{
    public Task<int> Add(BookingRequest bookingRequest)
    {
        return Task.FromResult(1);
    }
    public async Task<TId> Add<TRequest, TEntity, TId>(TRequest requestDto)
        where TEntity : class, IEntity<TId>
    {
        if (typeof(TId) == typeof(string))
        {
            string mockId = "1";
            return (TId)(object)mockId;
        }

        if (typeof(TId) == typeof(int))
        {
            int mockId = 1;
            return (TId)(object)mockId;
        }

        throw new NotSupportedException($"Type {typeof(TId).Name} is not supported");
    }



    public async Task<TResult?> Get<TId, TEntity, TResult>(TId id)
        where TEntity : class, IEntity<TId>
        where TResult : class
    {
        if (typeof(TEntity) == typeof(Booking))
        {
            int mockId = 1;
            if (!id!.Equals((TId)(object)mockId)) return null;
            List<RecyclingItemView> recyclingItems = new()
            {
                new() {MaterialType=MaterialTypes.aluminium, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
                new() {MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
                new() {MaterialType=MaterialTypes.glass, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},
            };
            var booking = new BookingView()
            {
                Status = BookingStatus.Scheduled,
                Schedule = new() { StartDate = new DateOnly(2026, 8, 20), IsDefault = false },
                Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
                Recycling = new() { BookingId = mockId, NumberOfBags = 2, RecyclingItems = recyclingItems },
                DateCreated = DateTime.Today,
            };

            return (TResult)(object)booking;
        }
        return null;
    }

    public async Task<TId> Update<TId, TRequest, TEntity>(TRequest request)
        where TEntity : class, IEntity<TId>
        where TRequest : class, IRequest<TId>
    {
        throw new NotImplementedException();
    }

}