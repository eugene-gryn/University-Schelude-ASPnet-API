using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class UserRepository : EFRepository<User>
{
    public UserRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<User> Create(User item)
    {
        item.Id = 0;
        item.IsAdmin = false;

        await Context.Users.AddAsync(item);
        return item;
    }

    public override IQueryable<User> Read()
    {
        return Context.Users.AsQueryable();
    }

    public override Task<bool> Update(User item)
    {
        Context.Entry(item).State = EntityState.Modified;
        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id)
    {
        await Context.DisposeAsync();

        Context.Users
            .Include(user => user.Homeworks)
            .Include(user => user.Groups);


        return true;
    }
}