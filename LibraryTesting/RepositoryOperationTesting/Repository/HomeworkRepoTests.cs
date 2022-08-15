using System;
using System.Linq;
using System.Threading.Tasks;
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
        await LoadRandomDataSet(10);

        var item = Generator.GenEmptyHomework(1, Uow.Subjects.Read().First()).First();

        var result = await Uow.Homework.Add(item);
        Uow.Save();

        result.Should().Be(true);
        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count + 1);
    }
    [Test]
    public async Task RangeCreation_Successful()
    {
        await LoadRandomDataSet(10);

        int COUNT = 4;
        var items = Generator.GenEmptyHomework(COUNT, Uow.Subjects.Read().First());

        var result = await Uow.Homework.AddRange(items);
        Uow.Save();

        result.Should().Be(true);
        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count + items.Count);
    }

    [Test]
    public async Task Update_FoundAndUpdateItem_Success()
    {
        await LoadRandomDataSet(3);
        var newProp = "this is!";

        var homeworkInfo = Generator.Homework.First();
        var homework = await Uow.Homework.ReadById(homeworkInfo.Id).FirstOrDefaultAsync();

        homework.Description = newProp;

        var res = await Uow.Homework.Update(homework);
        Uow.Save();

        homework = await Uow.Homework.ReadById(homeworkInfo.Id).FirstOrDefaultAsync();

        res.Should().BeTrue();
        homework.Should().NotBeNull();
        homework.Description.Should().Be(newProp);
    }

    [Test]
    public async Task Remove_AllDependedListCheck_Removed()
    {
        await LoadRandomDataSet(3);

        var user = Generator.Users.First();
        var homework = await Uow.Homework.ReadById(user.Homework.First().Id).FirstOrDefaultAsync();

        var res = await Uow.Homework.Delete(homework.Id);
        Uow.Save();

        res.Should().BeTrue();
        (Generator.Homework.Count - 1).Should().Be(Uow.Homework.Read().Count());
        (user.Homework.Count - 1).Should().Be(Uow.Users.ReadById(user.Id)
            .Include(u => u.Homework).First().Homework.Count);
    }
}