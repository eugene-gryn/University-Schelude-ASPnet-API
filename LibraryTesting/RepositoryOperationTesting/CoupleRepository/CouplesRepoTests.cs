using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

[TestFixture]
public class CouplesRepoTests : BaseRepositoryTest
{
    [Test]
    public async Task Creation_Successful()
    {
        await LoadRandomDataSet(10);

        var couple = Generator.GenEmptyCouple(1, Uow.Subjects.Read().First()).First();

        var result = await Uow.Couples.Add(couple);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count + 1);
    }
    [Test]
    public async Task RangeCreation_Successful()
    {
        await LoadRandomDataSet(10);

        int COUNT = 4;
        var couples = Generator.GenEmptyCouple(COUNT, new Subject()
        {
            OwnerGroup = Uow.Groups.Read().First()
        });

        var result = await Uow.Couples.AddRange(couples);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count + couples.Count);
    }
}