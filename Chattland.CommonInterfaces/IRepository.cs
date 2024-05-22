namespace Chattland.CommonInterfaces;

public interface IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    Task<TEntity> GetByIdAsync(TId id);
    Task<IEnumerable<TEntity>> GetManyAsync(int start, int count);
    Task AddOneAsync(TEntity item);
}