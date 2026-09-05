public class BookingRepositoryStub : IBookingRepository
{
    public async Task<TId> Add<TRequest, TEntity, TId>(TRequest requestDto)
        where TEntity : class, IEntity<TId>, new()
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
        where TEntity : class, IEntity<TId>, new()
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
                CollectionDate = new DateTime(2026, 8, 20),
                DateCreated = DateTime.Today,
                Location = new() { MapsId = "test", AddressLine1 = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
                RecyclingItems = recyclingItems
            };

            return (TResult)(object)booking;
        }
        return null;
    }

    public async Task<TId> Update<TId, TRequest, TEntity>(TRequest request)
        where TEntity : class, IEntity<TId>, new()
        where TRequest : class, IRequest<TId>
    {
        throw new NotImplementedException();
    }

}