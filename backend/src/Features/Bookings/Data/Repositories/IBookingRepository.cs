public interface IBookingRepository
{
    public Task<int> Add(BookingRequest bookingRequest);
    public Task<TId> Add<TRequest, TEntity, TId>(TRequest requestDto)
        where TEntity : class, IEntity<TId>;
    public Task<TResult?> Get<TId, TEntity, TResult>(TId id)
       where TEntity : class, IEntity<TId>
       where TResult : class;
    public Task<TId> Update<TId, TRequest, TEntity>(TRequest request)
        where TEntity : class, IEntity<TId>
        where TRequest : class, IRequest<TId>;
}