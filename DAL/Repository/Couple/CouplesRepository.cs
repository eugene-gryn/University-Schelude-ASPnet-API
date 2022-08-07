using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Couple;

public class CouplesRepository : EFRepository<Entities.Couple>, ICoupleRepository
{
    public CouplesRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<bool> Add(Entities.Couple item)
    {
        item.Id = 0;

        // TODO VALIDATIONS

        await Context.Couples.AddAsync(item);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Couple> entities)
    {
        var list = entities as Entities.Couple[] ?? entities.ToArray();
        foreach (var item in list)
        {
            item.Id = 0;

            // TODO VALIDATIONS
        }

        await Context.Couples.AddRangeAsync(list);

        return true;
    }

    public override IQueryable<Entities.Couple> ReadById(int id)
    {
        throw new NotImplementedException();
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