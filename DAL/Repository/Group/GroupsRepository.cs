using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Group;

public class GroupsRepository : EFRepository<Entities.Group>, IGroupRepository {
    public GroupsRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.Group item) {
        var group = new Entities.Group {
            UsersRoles = new List<UserRole>(),
            Couples = new List<Entities.Couple>(),
            Subjects = new List<Entities.Subject>(),
            Id = 0,
            Name = item.Name,
            PrivateType = item.PrivateType
        };

        await Context.AddAsync(group);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Group> entities) {
        var groups = entities.ToList();
        for (var i = 0; i < groups.Count; i++)
            groups[i] = new Entities.Group {
                UsersRoles = new List<UserRole>(),
                Couples = new List<Entities.Couple>(),
                Subjects = new List<Entities.Subject>(),
                Id = 0,
                Name = groups[i].Name,
                PrivateType = groups[i].PrivateType
            };

        await Context.Groups.AddRangeAsync(groups);

        return true;
    }

    public override IQueryable<Entities.Group> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> Update(Entities.Group item) {
        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id) {
        var group = await Context.Groups
            .Where(g => g.Id == id)
            .Include(group => group.Couples)
            .Include(group => group.UsersRoles)
            .Include(group => group.Subjects)
            .FirstOrDefaultAsync();

        if (group == null) return false;

        Context.Remove(group);

        return true;
    }

    public async Task<bool> AddUser(int groupId, int userId) {
        var group = await ReadById(groupId).FirstOrDefaultAsync();
        var user = await Context.Users.FirstOrDefaultAsync(usr => usr.Id == userId);

        if (group == null || user == null) return false;

        group.UsersRoles.Add(new UserRole { User = user });

        return await Update(group);
    }

    public async Task<bool> RemoveUser(int groupId, int userId) {
        var group = await ReadById(groupId).FirstOrDefaultAsync();
        var user = await ReadUserById(groupId, userId);

        if (group == null || user == null) return false;

        var userRemove = group.UsersRoles.Remove(user);
        if (!userRemove) return false;

        return await Update(group);
    }

    public async Task<Entities.Couple?> GetCouple(int groupId, int coupleId) {
        var group = await ReadById(groupId).Include(group => group.Couples).FirstOrDefaultAsync();

        var couple = group?.Couples.FirstOrDefault(couple => couple.Id == coupleId);

        return couple;
    }

    public async Task<Entities.Couple?> NearCouple(int groupId) {
        var group = await ReadById(groupId).Include(group => group.Couples).FirstOrDefaultAsync();

        var nearCouple = group?.Couples.OrderBy(couple => couple.Begin).First();

        return nearCouple;
    }

    public Task<List<Entities.Couple>> TodayCouples(int groupId) {
        throw new NotImplementedException();
    }

    public Task<bool> SetNewCreator(int groupId, int userId) {
        throw new NotImplementedException();
    }

    public Task<bool> AddModerator(int groupId, int userId) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveModerator(int groupId, int userId) {
        throw new NotImplementedException();
    }


    private async Task<UserRole?> ReadUserById(int groupId, int id) {
        return (await ReadById(groupId)
            .Include(group => group.UsersRoles
                .Select(role => role.User))
            .FirstOrDefaultAsync())?.UsersRoles.FirstOrDefault(role => role.User.Id == id);
    }
}