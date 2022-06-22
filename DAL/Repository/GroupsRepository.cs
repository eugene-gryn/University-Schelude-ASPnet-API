using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class GroupsRepository : EFRepository<Group>
{
    public GroupsRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<Group> Create(Group item)
    {
        item.Id = 0;
        item.Moderators.Clear();

        await Context.AddAsync(item);

        return item;
    }

    public override IQueryable<Group> Read()
    {
        return Context.Groups.AsQueryable();
    }

    public override async Task<bool> Update(Group item)
    {
        // MAYBE: Update(item)

        Context.Entry(item).State = EntityState.Modified;

        return true;
    }

    public override async Task<bool> Delete(int id)
    {
        var group = await Context.Groups
            .Where(g => g.Id == id)
            .Include(group => group.Couples
                .Select(couple => couple.Subject))
            .Include(group => group.Subjects)
            .FirstOrDefaultAsync();

        if (group != null)
        {
            Context.Remove(group);

            return true;
        }

        return false;
    }
}