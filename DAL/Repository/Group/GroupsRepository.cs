using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Group;

public class GroupsRepository : EFRepository<Entities.Group>, IGroupRepository
{
    public GroupsRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<bool> Add(Entities.Group item)
    {
        item.Id = 0;
        item.Moderators.Clear();

        await Context.AddAsync(item);

        return false;
    }

    public override Task<bool> AddRange(IEnumerable<Entities.Group> entities)
    {
        throw new NotImplementedException();
    }

    public override IQueryable<Entities.Group> Read()
    {
        return Context.Groups.AsQueryable();
    }

    public override async Task<bool> Update(Entities.Group item)
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

    public Task<bool> AddUser(int groupId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveUser(int groupId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Entities.Couple> GetCouple(int groupId, int coupleId)
    {
        throw new NotImplementedException();
    }

    public Task<Entities.Couple> NearCouple(int groupId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Entities.Couple>> TodayCouples(int groupId)
    {
        throw new NotImplementedException();
    }

    public Task<Entities.Group> GetFullGroup(int groupId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetNewCreator(int groupId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddModerator(int groupId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveModerator(int groupId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetName(int groupId, string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetPrivacy(int groupId, bool privacy)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddHomeworkTask(int groupId, HomeworkTask task)
    {
        throw new NotImplementedException();
    }
}