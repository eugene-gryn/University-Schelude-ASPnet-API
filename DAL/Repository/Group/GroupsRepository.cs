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

        if (item.Couples.Any()
            && item.Subjects.Any()
            && item.Users.Any()
            && item.Creator == null) return false;

        await Context.AddAsync(item);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Group> entities)
    {
        var groups = entities as Entities.Group[] ?? entities.ToArray();
        foreach (var item in groups)
        {
            item.Id = 0;
            item.Moderators.Clear();

            if (item.Couples.Any()
                && item.Subjects.Any()
                && item.Users.Any()
                && item.Creator == null) return false;
        }

        await Context.Groups.AddRangeAsync(groups);

        return true;
    }

    public override IQueryable<Entities.Group> Read()
    {
        return Context.Groups.AsQueryable();
    }

    public override IQueryable<Entities.Group> ReadById(int id)
    {
        return Read().Where(group => group.Id == id).AsQueryable();
    }

    public override Task<bool> Update(Entities.Group item)
    {
        // MAYBE: Update(item)

        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id)
    {
        var group = await Context.Groups
            .Where(g => g.Id == id)
            .Include(group => group.Couples
                .Select(couple => couple.Subject))
            .Include(group => group.Subjects)
            .FirstOrDefaultAsync();

        if (group == null) return false;

        Context.Remove(group);

        return true;
    }

    public async Task<bool> AddUser(int groupId, int userId)
    {
        var group = await ReadById(groupId).FirstOrDefaultAsync();
        var user = await ReadUserById(groupId).FirstOrDefaultAsync();

        if (group == null || user == null) return false;

        group.Users.Add(user);

        return await Update(group);
    }

    public async Task<bool> RemoveUser(int groupId, int userId)
    {
        var group = await ReadById(groupId).FirstOrDefaultAsync();
        var user = await ReadUserById(groupId).FirstOrDefaultAsync();

        if (group == null || user == null) return false;
        var userRemove = group.Users.Remove(user);
        if (!userRemove) return false;
        return await Update(group);
    }

    public async Task<Entities.Couple?> GetCouple(int groupId, int coupleId)
    {
        var group = await ReadById(groupId).Include(group => group.Couples).FirstOrDefaultAsync();


        if (group == null) return null;
        var couple = group.Couples.FirstOrDefault(couple => couple.Id == coupleId);

        return couple ?? null;
    }

    public async Task<Entities.Couple?> NearCouple(int groupId)
    {
        var group = await ReadById(groupId).Include(group => group.Couples).FirstOrDefaultAsync();

        if (group == null) throw null!;
        var nearCouple = group.Couples.OrderBy(couple => couple.Begin).First();

        return nearCouple;
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

    private IQueryable<Entities.User> ReadUserById(int id)
    {
        return Context.Users.Where(group => group.Id == id).AsQueryable();
    }
}