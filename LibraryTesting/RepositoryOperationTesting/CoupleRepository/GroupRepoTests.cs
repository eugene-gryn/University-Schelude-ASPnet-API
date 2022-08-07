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
        CreateGroup();
        var group = Generator.Groups.FirstOrDefault();

        var result = await Uow.Groups.Add(group);
        Uow.Save();

        result.Should().Be(true);
        Uow.Groups.Read().Count().Should().Be(Generator.Groups.Count);
    }

    [Test]
    public async Task RangeCreation_Successful()
    {
        int countOfGroups = 4;

        CreateGroup(countOfGroups);

        var result = await Uow.Groups.AddRange(Generator.Groups);
        Uow.Save();

        result.Should().Be(true);
        Uow.Groups.Read().Count().Should().Be(Generator.Groups.Count);
    }
}