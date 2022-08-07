using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

[TestFixture]
public class CouplesRepoTests : BaseRepositoryTest
{
    [Test]
    public async Task Creation_Successful()
    {
        CreateCouple();
        var couple = Generator.Couples.FirstOrDefault();

        var result = await Uow.Couples.Add(couple);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count);
    }
    [Test]
    public async Task RangeCreation_Successful()
    {
        int COUNT = 4;
        CreateCouple(COUNT);

        var result = await Uow.Couples.AddRange(Generator.Couples);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count);
    }
}