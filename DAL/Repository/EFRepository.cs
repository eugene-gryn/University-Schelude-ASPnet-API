using DAL.EF;

namespace DAL.Repository;

public abstract class EFRepository<TEntity> : IRepository<TEntity>
{
    protected ScheduleContext Context;

    protected EFRepository(ScheduleContext context)
    {
        Context = context;
    }

    public abstract Task<TEntity> Create(TEntity item);
    public abstract IQueryable<TEntity> Read();
    public abstract Task<bool> Update(TEntity item);
    public abstract Task<bool> Delete(int id);
}