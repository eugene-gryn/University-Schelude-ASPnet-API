using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using LibraryTesting.DataGenerator;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting;

[TestFixture]
public class BaseRepositoryTest : BaseTest
{
    protected override void EveryTimeSetUp()
    {
        base.EveryTimeSetUp();

        Generator.Clear();
    }

    public BaseRepositoryTest()
    {
        Generator = new ScheduleRandomGenerator();
    }

    protected ScheduleRandomGenerator Generator { get; }


    protected async Task LoadRandomDataSet(int countUser = 1)
    {
        Generator.MakeDataSet(countUser);

        if (!await Uow.Users.AddRange(Generator.Users)) return;

        if (!await Uow.Groups.AddRange(Generator.Groups)) return;

        if (!await Uow.Subjects.AddRange(Generator.Subjects)) return;

        if (!await Uow.Couples.AddRange(Generator.Couples)) return;

        if (!await Uow.Homework.AddRange(Generator.Homework)) return;

        Uow.Save();


        Generator.Groups.ForEach(async item =>
        {
            var group = Uow.Groups.ReadById(item.Id)
                .Include(g => g.Couples)
                .Include(g => g.Subjects)
                .Include(g => g.UsersRoles)
                .FirstOrDefault();

            if (group != null)
            {
                item.Couples.ToList()
                    .ForEach(couple => group.Couples.Add(Uow.Couples.ReadById(couple.Id).FirstOrDefault()));

                item.Subjects.ToList()
                    .ForEach(subj => group.Subjects.Add(Uow.Subjects.ReadById(subj.Id).FirstOrDefault()));

                item.UsersRoles.ToList().ForEach(async role =>
                {
                    group.UsersRoles.Add(new UserRole
                    {
                        Group = group,
                        GroupId = group.Id,
                        IsModerator = role.IsModerator,
                        IsOwner = role.IsOwner,
                        UserId = role.User.Id,
                        User = Uow.Users.ReadById(role.User.Id).FirstOrDefault()
                               ?? throw new InvalidOperationException()
                    });
                    await Uow.Groups.Update(group);
                    Uow.Save();
                });
            }

            await Uow.Groups.Update(group);
            Uow.Save();
        });

        UowUpdate();

        Generator.Users.ForEach(async item =>
        {
            var user = Uow.Users.ReadById(item.Id)
                .Include(u => u.UsersRoles)
                .Include(u => u.Homework)
                .FirstOrDefault();

            if (user != null)
            {
                item.Homework.ToList().ForEach(couple =>
                    user.Homework.Add(Uow.Homework.ReadById(couple.Id).FirstOrDefault()));
                user.Settings = item.Settings;
            }

            await Uow.Users.Update(user);
            Uow.Save();
        });
    }
}