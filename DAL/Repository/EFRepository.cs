using DAL.EF;

namespace DAL.Repository;

public abstract class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected ScheduleContext Context;

    protected EFRepository(ScheduleContext context)
    {
        Context = context;
    }

    public abstract Task<TEntity> Add(TEntity item);
    public abstract Task<bool> AddRange(IEnumerable<TEntity> entities);

    public virtual IQueryable<TEntity> Read()
    {
        return Context.Set<TEntity>().AsQueryable();
    }

    public abstract Task<bool> Update(TEntity item);
    public abstract Task<bool> Delete(int id);
}