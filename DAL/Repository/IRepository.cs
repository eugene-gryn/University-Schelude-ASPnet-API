namespace DAL.Repository;

public interface IRepository<TEntity>
{
    Task<bool> Add(TEntity item);
    Task<bool> AddRange (IEnumerable<TEntity> entities);
    bool Add(ref TEntity item);
    IQueryable<TEntity> Read();
    IQueryable<TEntity> ReadById(int id);
    Task<bool> UpdateAsync(TEntity item);
    bool Update(TEntity item);
    Task<bool> Delete(int id);
}