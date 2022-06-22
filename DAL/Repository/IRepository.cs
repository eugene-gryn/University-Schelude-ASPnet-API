namespace DAL.Repository;

public interface IRepository<TEntity>
{
    Task<TEntity> Create(TEntity item);
    IQueryable<TEntity> Read();
    Task<bool> Update(TEntity item);
    Task<bool> Delete(int id);
}