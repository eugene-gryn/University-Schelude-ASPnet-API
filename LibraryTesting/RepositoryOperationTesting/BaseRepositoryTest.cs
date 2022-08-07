using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using FluentAssertions;
using LibraryTesting.DataGenerator;
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
        var countUser = 10;

        await LoadRandomDataSet(countUser);

        Uow.Users.Read().Count().Should().Be(Generator.Users.Count);
        CollectionAssert.AreEquivalent(
            Generator.Users.Select(user => user.Id),
            Uow.Users.Read().Select(user => user.Id));

        Uow.Groups.Read().Count().Should().Be(Generator.Groups.Count);
        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count);
        Uow.Subjects.Read().Count().Should().Be(Generator.Subjects.Count);
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

        await Uow.Subjects.AddRange(Generator.Subjects);

        for (var i = 0; i < countUser; i++)
        {
        }

        Uow.Save();
    }
}