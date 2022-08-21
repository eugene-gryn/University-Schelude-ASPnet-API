using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using LibraryTesting.DataGenerator;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting;

[TestFixture]
public class BaseRepositoryTest : BaseTest {
    protected override void EveryTimeSetUp() {
        base.EveryTimeSetUp();

        Generator.Clear();
    }

    public BaseRepositoryTest() {
        Generator = new ScheduleRandomGenerator();
    }

    protected ScheduleRandomGenerator Generator { get; }


    protected async Task LoadRandomDataSet(int countUser = 1) {
        Generator.MakeDataSet(countUser);

        if (!await Uow.Users.AddRange(Generator.Users)) return;
        Uow.Save();

        if (!await Uow.Groups.AddRange(Generator.Groups)) return;
        Uow.Save();

        if (!await Uow.Subjects.AddRange(Generator.Subjects)) return;
        Uow.Save();

        if (!await Uow.Couples.AddRange(Generator.Couples)) return;
        Uow.Save();

        if (!await Uow.Homework.AddRange(Generator.Homework)) return;
        Uow.Save();



        Generator.Groups.ForEach(async item => {
            var group = Uow.Groups.ReadById(item.Id)
                .Include(g => g.UsersRoles)
                .FirstOrDefault();
        
            if (group != null)
            {
                item.UsersRoles.ToList().ForEach(async role => {
                    group.UsersRoles.Add(new UserRole
                    {
                        GroupId = group.Id,
                        IsModerator = role.IsModerator,
                        IsOwner = role.IsOwner,
                        UserId = role.User.Id,
                    });
                    await Uow.Groups.Update(group);
                    Uow.Save();
                });
            }
        
            await Uow.Groups.Update(group);
            Uow.Save();
        });
        
        UowUpdate();
        
        Generator.Users.ForEach(async item => {
            var user = Uow.Users.ReadById(item.Id)
                .FirstOrDefault();
        
            if (user != null) {
                user.Settings = item.Settings;
            }
        
            await Uow.Users.Update(user);
            Uow.Save();
        });

        UowUpdate();

        // COPY ARRAY
        
        Generator.Clear();
        
        Generator.Users = Uow.Users.Read()
            .Include(u => u.Homework)
            .ThenInclude(h => h.Subject)
            .ThenInclude(s => s.OwnerGroup)
            .Include(u => u.Settings)
            .Include(u => u.UsersRoles)
            .ThenInclude(u => u.Group)
            .ThenInclude(g => g.Couples)
            .ToList();
        
        Generator.Groups = Uow.Groups.Read()
            .Include(g => g.Couples)
            .Include(g => g.Subjects)
            .ThenInclude(s => s.Couples)
            .Include(g => g.Subjects)
            .ThenInclude(s => s.Homework)
            .Include(g => g.UsersRoles)
            .ToList();
        
        Generator.Subjects = Uow.Subjects.Read()
            .Include(s => s.Couples)
            .Include(s => s.Homework)
            .ThenInclude(h => h.User)
            .ThenInclude(u => u.UsersRoles)
            .ThenInclude(r => r.Group)
            .ThenInclude(g => g.Subjects)
            .Include(s => s.OwnerGroup)
            .ThenInclude(g => g.Couples)
            .ToList();
        
        Generator.Couples = Uow.Couples.Read()
            .Include(c => c.Subject)
            .ThenInclude(s => s.OwnerGroup)
            .ThenInclude(g => g.Subjects)
            .ToList();
        
        Generator.Homework = Uow.Homework.Read()
            .Include(h => h.User)
            .ThenInclude(u => u.UsersRoles)
            .Include(h => h.Subject)
            .ThenInclude(s => s.Couples)
            .ToList();
    }

    protected User RegisterNewUser() {
        var user = Generator.GenEmptyUsers(1).First();
        Uow.Users.Add(ref user);
        return user;
    }
    protected async Task<User> AddUser() {
        var user = Generator.GenEmptyUsers(1).First();

        var addResult = await Uow.Users.Add(user);

        if (!addResult) return null;

        Uow.Save();

        user = Uow.Users.Read().Last();

        return user;
    }
}