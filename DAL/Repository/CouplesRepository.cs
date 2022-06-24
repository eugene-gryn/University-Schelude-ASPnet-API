using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class CouplesRepository : EFRepository<Couple>
{
    public CouplesRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<Couple> Create(Couple item)
    {
        item.Id = 0;

        await Context.Couples.AddAsync(item);
    }

    public override IQueryable<Couple> Read()
    {
        return Context.Couples.AsQueryable();
    }

    public override Task<bool> Update(Couple item)
    {
        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id)
    {
        var couple = await Context.Couples.Where(coupleT => coupleT.Id == id).FirstOrDefaultAsync();

        if (couple != null)
        {
            Context.Remove(couple);
            return true;
        }

        return false;
    }
}