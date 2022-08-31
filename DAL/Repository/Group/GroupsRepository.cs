using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Group;

public class GroupsRepository : EFRepository<Entities.Group>, IGroupRepository {
    public GroupsRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.Group item) {
        var group = MapAdd(item);

        await Context.AddAsync(group);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Group> entities) {
        var groups = entities.ToList();
        for (var i = 0; i < groups.Count; i++)
            groups[i] = MapAdd(groups[i]);

        await Context.Groups.AddRangeAsync(groups);

        return true;
    }

    public override bool Add(ref Entities.Group item) {
        var group = MapAdd(item);

        item = Context.Add(group).Entity;

        return true;
    }


    public override IQueryable<Entities.Group> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> UpdateAsync(Entities.Group item) {
        Context.Entry(item).State = EntityState.Modified;
        
        return Task.FromResult(true);
    }

    public override bool Update(Entities.Group item) {
        Context.Entry(item).State = EntityState.Modified;
        
        return true;
    }

    public override async Task<bool> Delete(int id) {
        var group = await Context.Groups
            .Where(g => g.Id == id)
            .Include(group => group.Couples)
            .Include(group => group.UsersRoles)
            .Include(group => group.Subjects)
            .SingleOrDefaultAsync();

        if (group == null) return false;

        Context.Remove(group);

        return true;
    }

    public async Task<bool> AddUser(int groupId, int userId) {
        var group = await ReadById(groupId).SingleOrDefaultAsync();
        var user = await Context.Users.SingleOrDefaultAsync(usr => usr.Id == userId);

        if (group == null || user == null) return false;

        group.UsersRoles.Add(new UserRole {
            User = user,
            UserId = userId,
            Group = group,
            GroupId = groupId
        });

        return await UpdateAsync(group);
    }

    public async Task<bool> RemoveUser(int groupId, int userId) {
        var group = await ReadById(groupId).SingleOrDefaultAsync();
        var user = await ReadUserRoleByUserId(groupId, userId);

        if (group == null || user == null) return false;

        var userRemove = group.UsersRoles.Remove(user);
        if (!userRemove) return false;

        return await UpdateAsync(group);
    }

    public async Task<Entities.Couple?> NearCouple(int groupId) {
        var group = await ReadById(groupId)
            .Include(group => group.Couples)
            .SingleOrDefaultAsync();

        var nearCouple = group?.Couples.MinBy(couple => couple.Begin);

        return nearCouple;
    }

    public async Task<List<Entities.Couple>> DayCouples(int groupId, DateTime date) {
        var group = await ReadById(groupId).Include(g => g.Couples).SingleOrDefaultAsync();

        if (group == null) return new List<Entities.Couple>();

        return group.Couples.Where(c => c.Begin.Date == date.Date).ToList();
    }

    public async Task<bool> SetNewCreator(int groupId, int userId) {
        var g = await ReadById(groupId)
            .Include(g => g.UsersRoles)
            .SingleOrDefaultAsync();

        var ownerRole = g?.UsersRoles.SingleOrDefault(role => role.IsOwner);

        if (ownerRole == null || g == null) return false;

        var userRole = g.UsersRoles.SingleOrDefault(role => role.UserId == userId);

        if (userRole == null)
            g.UsersRoles.Add(new UserRole {
                GroupId = g.Id,
                IsModerator = false,
                IsOwner = true,
                UserId = userId
            });
        else
            userRole.IsOwner = true;

        ownerRole!.IsOwner = false;

        return await UpdateAsync(g);
    }

    public async Task<bool> AddModerator(int groupId, int userId) {
        var g = await ReadById(groupId)
            .Include(g => g.UsersRoles)
            .SingleOrDefaultAsync();

        var userRole = g?.UsersRoles.SingleOrDefault(role => role.UserId == userId);

        if (g == null) return false;

        if (userRole == null)
            g.UsersRoles.Add(new UserRole {
                GroupId = g.Id,
                IsModerator = true,
                IsOwner = false,
                UserId = userId
            });
        else
            userRole.IsModerator = true;

        return await UpdateAsync(g);
    }

    public async Task<bool> RemoveModerator(int groupId, int userId) {
        var g = await ReadById(groupId)
            .Include(g => g.UsersRoles)
            .SingleOrDefaultAsync();

        var userRole = g?.UsersRoles.SingleOrDefault(role => role.UserId == userId);

        if (g == null || userRole == null) return false;

        userRole.IsModerator = false;

        return await UpdateAsync(g);
    }

    protected override Entities.Group MapAdd(Entities.Group item) {
        return new Entities.Group {
            UsersRoles = new List<UserRole>(),
            Couples = new List<Entities.Couple>(),
            Subjects = new List<Entities.Subject>(),
            Id = 0,
            Name = item.Name,
            PrivateType = item.PrivateType
        };
    }


    private async Task<UserRole?> ReadUserRoleByUserId(int groupId, int id) {
        return (await ReadById(groupId)
            .Include(group => group.UsersRoles)
            .FirstOrDefaultAsync())?.UsersRoles.SingleOrDefault(role => role.UserId == id);
    }
}