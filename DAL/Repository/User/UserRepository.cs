using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.User;

public class UserRepository : EFRepository<Entities.User>, IUserRepository {
    public UserRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.User item) {
        var user = MapAdd(item);

        if (Context.Users.Any(u => u.Login == user.Login)) return false;

        await Context.Users.AddAsync(user);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.User> entities) {
        var enumerable = entities.ToList();

        for (var i = 0; i < enumerable.Count; i++) {
            if (Context.Users.Any(u => u.Login == enumerable[i].Login)) return false;

            enumerable[i] = MapAdd(enumerable[i]);
        }

        await Context.Users.AddRangeAsync(enumerable);

        return true;
    }

    public override IQueryable<Entities.User> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override async Task<bool> UpdateAsync(Entities.User item) {
        if (await Context.Users.AnyAsync(u => u.Id == item.Id ? u.Login != item.Login : u.Login == item.Login))
            return false;

        Context.Entry(item).State = EntityState.Modified;

        Context.Users.Update(item);

        return true;
    }

    public override bool Update(Entities.User item) {
        if (Context.Users.Any(u => u.Id == item.Id ? u.Login != item.Login : u.Login == item.Login)) return false;

        Context.Entry(item).State = EntityState.Modified;

        Context.Users.Update(item);

        return true;
    }

    public override async Task<bool> Delete(int id) {
        var user = await Context.Users.Where(userQuery => userQuery.Id == id)
            .Include(user => user.Homework)
            .Include(user => user.UsersRoles)
            .FirstOrDefaultAsync();


        if (user == null) return false;

        var userOwnedGroups = user.UsersRoles.Where(role => role.IsOwner).Select(role => role.Group).ToList();

        if (userOwnedGroups.All(g => g != null)) Context.Groups.RemoveRange(userOwnedGroups!);
        Context.Homework.RemoveRange(user.Homework);
        Context.Users.Remove(user);

        return true;
    }

    public async Task<List<UserRole>> GetUserGroups(int userId) {
        var list = await ReadById(userId).SelectMany(u => u.UsersRoles).Include(ur => ur.Group).ToListAsync();

        list.ForEach(l => l.Group?.UsersRoles.Clear());

        return list;
    }

    public async Task<int> GroupCount(int userId) {
        return await ReadById(userId).SelectMany(u => u.UsersRoles).Select(ur => ur).CountAsync();
    }

    public async Task<bool> GroupJoin(int userId, int groupId, bool fullAccess) {
        var entity = await ReadById(userId).SingleOrDefaultAsync();
        var group = await Context.Groups.SingleOrDefaultAsync(g => g.Id == groupId);

        if (entity == null || group == null) return false;

        if (!fullAccess || group.PrivateType) return false;

        entity.UsersRoles.Add(new UserRole {
            UserId = userId,
            GroupId = groupId,
            User = entity,
            Group = group,
            IsModerator = false,
            IsOwner = false
        });

        return await UpdateAsync(entity);
    }

    public async Task<bool> GroupLeave(int userId, int groupId) {
        var entity = await ReadById(userId)
            .SelectMany(u => u.UsersRoles).Where(ur => ur.UserId == userId && ur.GroupId == groupId)
            .SingleOrDefaultAsync();

        var user = await ReadById(userId).Include(u => u.UsersRoles).SingleOrDefaultAsync();

        if (entity == null || user == null) return false;

        user.UsersRoles.Remove(entity);

        return await UpdateAsync(user);
    }

    public override bool Add(ref Entities.User item) {
        var user = MapAdd(item);

        if (Context.Users.Any(u => u.Login == user.Login)) return false;

        item = Context.Users.Add(user).Entity;

        return true;
    }

    protected override Entities.User MapAdd(Entities.User item) {
        return new Entities.User {
            Id = 0,
            IsAdmin = false,
            UsersRoles = new List<UserRole>(),
            Homework = new List<HomeworkTask>(),
            ProfileImage = item.ProfileImage,
            Login = item.Login,
            Name = item.Name,
            Password = item.Password,
            Salt = item.Salt
        };
    }
}