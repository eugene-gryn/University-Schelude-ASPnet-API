using System.Diagnostics;
using System.Reflection.Emit;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;

namespace RandomDataGenerator.DataGenerator;

public static class FillDbDataSet {
    public static async Task FillUowData(IUnitOfWork uow, ScheduleRandomGenerator generator, bool refillGenerator = true) {
        if (!await uow.Users.AddRange(generator.Users!)) return;
        uow.Save();

        if (!await uow.Groups.AddRange(generator.Groups)) return;
        uow.Save();

        if (!await uow.Subjects.AddRange(generator.Subjects)) return;
        uow.Save();

        if (!await uow.Couples.AddRange(generator.Couples)) return;
        uow.Save();

        if (!await uow.Homework.AddRange(generator.Homework)) return;
        uow.Save();



        generator.Groups.ForEach(item => {
            var group = uow.Groups.ReadById(item.Id)
                .Include(g => g.UsersRoles)
                .FirstOrDefault();

            if (group != null)
            {
                item.UsersRoles.ToList().ForEach(role => {
                    group.UsersRoles.Add(new UserRole
                    {
                        GroupId = group.Id,
                        IsModerator = role.IsModerator,
                        IsOwner = role.IsOwner,
                        UserId = role.UserId,
                    });
                    uow.Groups.Update(group);
                    uow.Save();
                });
            }

            if (group != null) uow.Groups.Update(group);
            uow.Save();
        });

        generator.Users.ForEach(item => {
            var user = uow.Users.ReadById(item.Id)
                .FirstOrDefault();

            if (user != null)
            {
                user.Settings = item.Settings;
            }

            if (user != null) uow.Users.Update(user);
            uow.Save();
        });


        // COPY ARRAY
        if (refillGenerator) {
            generator.Clear();

            generator.Users = uow.Users.Read()
                .Include(u => u.Homework)
                .ThenInclude(h => h.Subject)
                .ThenInclude(s => s!.OwnerGroup)
                .Include(u => u.Settings)
                .Include(u => u.UsersRoles)
                .ThenInclude(u => u.Group)
                .ThenInclude(g => g!.Couples)
                .ToList();

            generator.Groups = uow.Groups.Read()
                .Include(g => g.Couples)
                .Include(g => g.Subjects)
                .ThenInclude(s => s.Couples)
                .Include(g => g.Subjects)
                .ThenInclude(s => s.Homework)
                .Include(g => g.UsersRoles)
                .ToList();

            generator.Subjects = uow.Subjects.Read()
                .Include(s => s.Couples)
                .Include(s => s.Homework)
                .ThenInclude(h => h.User)
                .ThenInclude(u => u!.UsersRoles)
                .ThenInclude(r => r.Group)
                .ThenInclude(g => g!.Subjects)
                .Include(s => s.OwnerGroup)
                .ThenInclude(g => g!.Couples)
                .ToList();

            generator.Couples = uow.Couples.Read()
                .Include(c => c.Subject)
                .ThenInclude(s => s!.OwnerGroup)
                .ThenInclude(g => g!.Subjects)
                .ToList();

            generator.Homework = uow.Homework.Read()
                .Include(h => h.User)
                .ThenInclude(u => u!.UsersRoles)
                .Include(h => h.Subject)
                .ThenInclude(s => s!.Couples)
                .ToList();
        }
    }

}