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
    public abstract Task<bool> Add(out TEntity item);

    public abstract Task<bool> AddRange(out IEnumerable<TEntity> entities);

    public virtual IQueryable<TEntity> Read()
    {
        return Context.Set<TEntity>().AsQueryable();
    }

    public abstract IQueryable<TEntity> ReadById(int id);

    public abstract Task<bool> Update(TEntity item);
    public abstract Task<bool> Delete(int id);
}