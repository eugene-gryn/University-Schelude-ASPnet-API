using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.User;

public class UserRepository : EFRepository<Entities.User>, IUserRepository {
    public UserRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.User item) {
        var user = new Entities.User {
            Id = 0,
            IsAdmin = false,
            UsersRoles = new List<UserRole>(),
            Homework = new List<HomeworkTask>(),
            ImageLocation = item.ImageLocation,
            Login = item.Login,
            Name = item.Name,
            Password = item.Password,
            Salt = item.Salt
        };

        await Context.Users.AddAsync(user);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.User> entities) {
        var enumerable = entities.ToList();
        for (var i = 0; i < enumerable.Count; i++)
            enumerable[i] = new Entities.User {
                Id = 0,
                IsAdmin = false,
                UsersRoles = new List<UserRole>(),
                Homework = new List<HomeworkTask>(),
                ImageLocation = enumerable[i].ImageLocation,
                Login = enumerable[i].Login,
                Name = enumerable[i].Name,
                Password = enumerable[i].Password,
                Salt = enumerable[i].Salt
            };

        await Context.Users.AddRangeAsync(enumerable);

        return true;
    }

    public override Task<bool> Add(out Entities.User item) {
        throw new NotImplementedException();
    }

    public override Task<bool> AddRange(out IEnumerable<Entities.User> entities) {
        throw new NotImplementedException();
    }

    public override IQueryable<Entities.User> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> Update(Entities.User item) {
        Context.Entry(item).State = EntityState.Modified;

        Context.Users.Update(item);

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id) {
        var user = await Context.Users.Where(userQuery => userQuery.Id == id)
            .Include(user => user.Homework)
            .Include(user => user.UsersRoles)
            .FirstOrDefaultAsync();


        if (user == null) return false;

        var userOwnedGroups = user.UsersRoles.Where(role => role.IsOwner).Select(role => role.Group);

        Context.Groups.RemoveRange(userOwnedGroups);
        Context.Homework.RemoveRange(user.Homework);
        Context.Users.Remove(user);

        return true;

    }
}