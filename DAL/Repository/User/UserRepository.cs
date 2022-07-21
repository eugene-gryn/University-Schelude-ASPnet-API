using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.User;

public class UserRepository : EFRepository<Entities.User>
{
    public UserRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<Entities.User> Create(Entities.User item)
    {
        item.Id = 0;
        item.IsAdmin = false;

        await Context.Users.AddAsync(item);
        return item;
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
}