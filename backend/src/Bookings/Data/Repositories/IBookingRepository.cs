public interface IBookingRepository
{
    public Task<TId> Add<TRequest, TEntity, TId>(TRequest requestDto)
        where TEntity : class, IEntity<TId>, new();
    public Task<TResult?> Get<TId, TEntity, TResult>(TId id)
       where TEntity : class, IEntity<TId>, new()
       where TResult : class;
    public Task<TId> Update<TId, TRequest, TEntity>(TRequest request)
        where TEntity : class, IEntity<TId>, new()
        where TRequest : class, IRequest<TId>;
}