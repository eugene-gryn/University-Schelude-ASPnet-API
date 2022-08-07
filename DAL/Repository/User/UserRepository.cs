using System.Linq;
using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.User;

public class UserRepository : EFRepository<Entities.User>, IUserRepository
{
    public UserRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<bool> Add(Entities.User item)
    {
        item.Id = 0;
        item.IsAdmin = false;

        if (item.Groups.Any() && item.Homeworks.Any()) return false;

        await Context.Users.AddAsync(item);
        
        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.User> entities)
    {
        foreach (var item in entities)
        {
            item.Id = 0;
            item.IsAdmin = false;

            if (item.Groups.Any() && item.Homeworks.Any()) return false;
        }

        await Context.Users.AddRangeAsync(entities);

        return true;
    }

    public override IQueryable<Entities.User> ReadById(int id)
    {
        return Read().Where(user => user.Id == id).AsQueryable();
    }

    public override Task<bool> Update(Entities.User item)
    {
        Context.Entry(item).State = EntityState.Modified;
        return Task.FromResult(true);
    }

    // TODO: глянуть удаляется ли таблицы USERS, MODERATOR
    public override async Task<bool> Delete(int id)
    {
        var user = await Context.Users.Where(userQuery => userQuery.Id == id)
            .Include(user => user.Homeworks
                .Select(homework => homework.Subject))
            .Include(user => user.Groups
                .Select(group => group.Creator))
            .FirstOrDefaultAsync();

        if (user != null)
        {
            Context.Users.Remove(user);

            return true;
        }

        return false;
    }

    public async Task<bool> SetName(int id, string name)
    {
        var user = await ReadById(id).FirstOrDefaultAsync();

        if (user != null)
        {
            user.Name = name;
            return await Update(user);
        }

        return false;
    }

    public async Task<bool> SetPicture(int id, string picUrl)
    {
        var user = await ReadById(id).FirstOrDefaultAsync();

        if (user != null)
        {
            user.ImageLocation = picUrl;
            return await Update(user);
        }

        return false;
    }

    public async Task<bool> SetPassword(int id, byte[] password, byte[] salt)
    {
        var user = await ReadById(id).FirstOrDefaultAsync();

        if (user != null)
        {
            user.Password = password;
            user.Salt = salt;
            return await Update(user);
        }

        return false;
    }

    public async Task<bool> MakeAdmin(int id)
    {
        var user = await ReadById(id).FirstOrDefaultAsync();

        if (user != null)
        {
            user.IsAdmin = true;
            return await Update(user);
        }

        return false;
    }

    public async Task<bool> RemoveAdmin(int id)
    {
        var user = await ReadById(id).FirstOrDefaultAsync();

        if (user != null)
        {
            user.IsAdmin = false;
            return await Update(user);
        }

        return false;
    }

    public async Task<bool> SetSettings(int id, Settings settings)
    {
        var user = await ReadById(id).FirstOrDefaultAsync();

        if (user != null)
        {
            user.Settings = settings;
            return await Update(user);
        }

        return false;
    }
}