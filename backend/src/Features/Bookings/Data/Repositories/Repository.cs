using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class Repository : IRepository
{
    protected ApplicationDbContext _context;
    protected readonly IMapper _mapper;
    public Repository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TId> Add<TRequest, TEntity, TId>(TRequest requestDto)
    where TEntity : class, IEntity<TId>
    {
        var entity = _mapper.Map<TEntity>(requestDto);
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }
    public async Task<TResult?> Get<TId, TEntity, TResult>(TId id)
        where TEntity : class, IEntity<TId>
        where TResult : class
    {
        return await _context.Set<TEntity>().Where(x => x.Id!.Equals(id))
        .ProjectTo<TResult>(_mapper.ConfigurationProvider)
        .SingleOrDefaultAsync();
    }
    public async Task<TId> Update<TId, TRequest, TEntity>(TRequest request)
    where TEntity : class, IEntity<TId>
    where TRequest : class, IRequest<TId>
    {
        var entity = await _context.Set<TEntity>().FindAsync(request.Id);
        if (entity is null)
        {
            throw new Exception($"There is no {nameof(TEntity)} with id {request.Id}");
        }
        _mapper.Map<TRequest, TEntity>(request);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

}