﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.Repository;

[TestFixture]
public class HomeworkRepoTests : BaseRepositoryTest
{
    [Test]
    public async Task Creation_Successful()
    {
        await GenerateRandomDataSet(10);

        var subject = Uow.Subjects.Read()
            .Include(s => s.OwnerGroup)
            .ThenInclude(g => g.UsersRoles)
            .ThenInclude(r => r.User)
            .First();

        var item = Generator.GenEmptyHomework(1,
            subject,
            subject.OwnerGroup.UsersRoles.First().User).First();

        var result = await Uow.Homework.Add(item);
        Uow.Save();

        result.Should().Be(true);
        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count + 1);
    }
    [Test]
    public async Task RangeCreation_Successful()
    {
        await GenerateRandomDataSet(10);

        int COUNT = 4;
        var subject = Uow.Subjects.Read()
            .Include(s => s.OwnerGroup)
            .ThenInclude(g => g.UsersRoles)
            .ThenInclude(r => r.User)
            .First();

        var items = Generator.GenEmptyHomework(COUNT,
            subject,
            subject.OwnerGroup.UsersRoles.First().User);

        var result = await Uow.Homework.AddRange(items);
        Uow.Save();

        result.Should().BeTrue();
        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count + items.Count);
    }

    [Test]
    public async Task Update_FoundAndUpdateItem_Success()
    {
        await GenerateRandomDataSet(3);
        var newProp = "this is!";

        var homeworkInfo = Generator.Homework.First();
        var homework = await Uow.Homework.ReadById(homeworkInfo.Id).FirstOrDefaultAsync();

        homework.Description = newProp;

        var res = await Uow.Homework.UpdateAsync(homework);
        Uow.Save();

        homework = await Uow.Homework.ReadById(homeworkInfo.Id).FirstOrDefaultAsync();

        res.Should().BeTrue();
        homework.Should().NotBeNull();
        homework.Description.Should().Be(newProp);
    }

    [Test]
    public async Task Remove_AllDependedListCheck_Removed()
    {
        await GenerateRandomDataSet(3);

        var user = Generator.Users.First();
        var homework = await Uow.Homework.ReadById(user.Homework.First().Id).FirstOrDefaultAsync();

        var res = await Uow.Homework.Delete(homework.Id);
        Uow.Save();

        res.Should().BeTrue();
        (Generator.Homework.Count - 1).Should().Be(Uow.Homework.Read().Count());
    }

    [Test]
    public async Task SetSubject_ChangeSubjectOnHomework_Successful() {
        await GenerateRandomDataSet(2);
        var group = await Uow.Groups.ReadById(1)
            .Include(g => g.Subjects)
            .ThenInclude(s => s.Homework)
            .ThenInclude(h => h.User)
            .Include(g => g.UsersRoles)
            .ThenInclude(r => r.User)
            .ThenInclude(u => u.Homework)
            .SingleOrDefaultAsync();
        var user = group!.UsersRoles.First().User;
        var homeworkTask = user.Homework.First();
        var subject = group.Subjects.First(s => s.Homework.All(h => h.Id != homeworkTask.Id));

        var result = await Uow.Homework.SetSubject(homeworkTask.Id, subject.Id);
        Uow.Save();

        result.Should().BeTrue();
        group.Should().NotBeNull();
        Uow.Homework.ReadById(homeworkTask.Id).Single().SubjectId.Should().Be(subject.Id);
    }
}