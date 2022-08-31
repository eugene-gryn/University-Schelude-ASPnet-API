using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public abstract class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected ScheduleContext Context;

    protected EFRepository(ScheduleContext context)
    {
        Context = context;
    }

    public abstract Task<bool> Add(TEntity item);
    public abstract Task<bool> AddRange(IEnumerable<TEntity> entities);
    public abstract bool Add(ref TEntity item);


    public virtual IQueryable<TEntity> Read()
    {
        return Context.Set<TEntity>().AsQueryable();
    }
    protected abstract TEntity MapAdd(TEntity entity);

    public abstract IQueryable<TEntity> ReadById(int id);

    public abstract Task<bool> UpdateAsync(TEntity item);
    public abstract bool Update(TEntity item);

    public abstract Task<bool> Delete(int id);
}