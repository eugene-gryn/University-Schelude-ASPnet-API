using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using FluentAssertions;
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
        Generator = new ScheduleDataSetGenerator();
    }

    protected ScheduleDataSetGenerator Generator { get; }

    protected void CreateUser(int count = 1)
    {
        var users = Generator.RUsers(count);

        Generator.Users.AddRange(users);
    }

    protected void CreateGroup(int count = 1, User creator = null)
    {
        var groups = new List<Group>(count);

        for (var i = 0; i < count; i++)
            groups.Add(Generator.GroupGenerate(0, creator, new List<Subject>(),
                new List<User>(), new List<User>(), new List<Couple>()));

        Generator.Groups.AddRange(groups);
    }


    protected void CreateCouple(int count = 1, Subject subject = null)
    {
        var couples = new List<Couple>(count);

        for (var i = 0; i < count; i++)
            couples.Add(Generator.CoupleGenerate(Generator.Couples.Count, subject));

        Generator.Couples.AddRange(couples);
    }

    protected void CreateSubject(int count = 1, Group group = null)
    {
        var subjects = new List<Subject>(count);

        for (var i = 0; i < count; i++)
            subjects.Add(Generator.SubjectGenerate(Generator.Subjects.Count, group));

        Generator.Subjects.AddRange(subjects);
    }

    protected void CreateHomework(int count = 1, Subject subject = null)
    {
        var homeworkTasks = new List<HomeworkTask>(count);

        for (var i = 0; i < count; i++)
            homeworkTasks.Add(Generator.HomeworkGenerate(Generator.Homework.Count, subject));

        Generator.Homework.AddRange(homeworkTasks);
    }

    [Test]
    public async Task FullLoadDataSet_CorrectLoading()
    {
        var countUser = 20;

        await LoadRandomDataSet(countUser);

        Uow.Users.Read().Count().Should().Be(Generator.Users.Count);
        CollectionAssert.AreEquivalent(
            Generator.Users.Select(item => item.Id),
            Uow.Users.Read().Select(item => item.Id));

        Uow.Groups.Read().Count().Should().Be(Generator.Groups.Count);
        CollectionAssert.AreEquivalent(
            Generator.Groups.Select(item => item.Id),
            Uow.Groups.Read().Select(item => item.Id));

        Uow.Subjects.Read().Count().Should().Be(Generator.Subjects.Count);
        CollectionAssert.AreEquivalent(
            Generator.Subjects.Select(item => item.Id),
            Uow.Subjects.Read().Select(item => item.Id));

        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count);
        CollectionAssert.AreEquivalent(
            Generator.Couples.Select(item => item.Id),
            Uow.Couples.Read().Select(item => item.Id));

        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count);
        CollectionAssert.AreEquivalent(
            Generator.Homework.Select(item => item.Id),
            Uow.Homework.Read().Select(item => item.Id));

        Uow.Users.Read()
            .Include(user => user.Homeworks)
            .ToList()
            .Any(item => item.Homeworks.Count > 0).Should().BeTrue();

        Uow.Users.Read()
            .Include(user => user.Groups)
            .ToList()
            .Any(user => user.Groups.Count > 0).Should().BeTrue();

        Uow.Groups.Read()
            .Include(item => item.Creator)
            .Include(item => item.Users)
            .Include(item => item.Moderators)
            .Include(item => item.Subjects)
            .Include(item => item.Couples)
            .ToList()
            .Any(item => item.Creator != null
                         && item.Users.Count > 0
                         && item.Moderators.Count > 0
                         && item.Subjects.Count > 0
                         && item.Couples.Count > 0).Should().BeTrue();
    }

    protected async Task LoadRandomDataSet(int countUser = 1)
    {
        Generator.RDataSet(countUser);

        var users = new List<User>(Generator.Users);
        users.ForEach(user =>
        {
            user.Groups.Clear();
            user.Homeworks.Clear();
        });

        if (!await Uow.Users.AddRange(users)) return;

        var groups = new List<Group>(Generator.Groups);
        groups.ForEach(group =>
        {
            group.Users.Clear();
            group.Couples.Clear();
            group.Subjects.Clear();
            group.Moderators.Clear();
        });

        if (!await Uow.Groups.AddRange(groups)) return;

        if (!await Uow.Subjects.AddRange(Generator.Subjects)) return;

        if (!await Uow.Couples.AddRange(Generator.Couples)) return;

        if (!await Uow.Homework.AddRange(Generator.Homework)) return;

        Generator.Groups.ForEach(item => Uow.Groups.Update(item));
        
        Generator.Users.ForEach(item => Uow.Users.Update(item));

        Uow.Save();
    }
}