using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

[TestFixture]
public class GroupRepoTests : BaseRepositoryTest
{
    [Test]
    public async Task Creation_Successful()
    {
        var group = Generator.GenEmptyGroups(1).First();

        var result = await Uow.Groups.Add(group);
        Uow.Save();

        result.Should().Be(true);
        Uow.Groups.Read().Count().Should().Be(1);
    }

    [Test]
    public async Task RangeCreation_Successful()
    {
        int countOfGroups = 4;

        var groups = Generator.GenEmptyGroups(countOfGroups);

        var result = await Uow.Groups.AddRange(groups);
        Uow.Save();

        result.Should().Be(true);
        Uow.Groups.Read().Count().Should().Be(groups.Count);
    }
}