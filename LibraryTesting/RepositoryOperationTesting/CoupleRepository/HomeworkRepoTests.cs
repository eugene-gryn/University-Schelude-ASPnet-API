using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

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
}