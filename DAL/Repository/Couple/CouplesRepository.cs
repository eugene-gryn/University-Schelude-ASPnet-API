using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Couple;

public class CouplesRepository : EFRepository<Entities.Couple>, ICoupleRepository
{
    public CouplesRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<Entities.Couple> Create(Entities.Couple item)
    {
        item.Id = 0;

        await Context.Couples.AddAsync(item);

        return item;
    }

    public override Task<bool> Update(Entities.Couple item)
    {
        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    // TODO: TEST TO AUTO REMOVE FROM LIST
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

    public Task<bool> RemoveAll(int groupId)
    {
        throw new NotImplementedException();
    }
}