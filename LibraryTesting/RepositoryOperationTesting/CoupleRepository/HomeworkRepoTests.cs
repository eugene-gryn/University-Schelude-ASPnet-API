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
        CreateHomework();
        var item = Generator.Homework.FirstOrDefault();

        var result = await Uow.Homework.Add(item);
        Uow.Save();

        result.Should().Be(true);
        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count);
    }
    [Test]
    public async Task RangeCreation_Successful()
    {
        int COUNT = 4;
        CreateHomework(COUNT);

        var result = await Uow.Homework.AddRange(Generator.Homework);
        Uow.Save();

        result.Should().Be(true);
        Uow.Homework.Read().Count().Should().Be(Generator.Homework.Count);
    }
}