using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.Repository;

[TestFixture]
public class GroupRepoTests : BaseRepositoryTest {
    [Test]
    public async Task Creation_Successful() {
        var group = Generator.GenEmptyGroups().First();

        var result = await Uow.Groups.Add(group);
        Uow.Save();

        result.Should().Be(true);
        Uow.Groups.Read().Count().Should().Be(1);
    }

    [Test]
    public async Task RangeCreation_Successful() {
        var countOfGroups = 4;

        var groups = Generator.GenEmptyGroups(countOfGroups);

        var result = await Uow.Groups.AddRange(groups);
        Uow.Save();

        result.Should().Be(true);
        Uow.Groups.Read().Count().Should().Be(groups.Count);
    }

    [Test]
    public async Task Update_FoundAndUpdateItem_Success() {
        await LoadRandomDataSet(2);
        var newProp = "NameGroup: Valid";

        var groupInfo = Generator.Groups.FirstOrDefault();
        var group = await Uow.Groups.ReadById(groupInfo.Id).FirstOrDefaultAsync();

        group.Name = newProp;

        var res = await Uow.Groups.Update(group);
        Uow.Save();

        group = await Uow.Groups.ReadById(groupInfo.Id).FirstOrDefaultAsync();

        res.Should().BeTrue();
        group.Should().NotBeNull();
        group.Name.Should().Be(newProp);
    }

    [Test]
    public async Task Remove_CheckAllDependedEntityRemoved_ValidRemove() {
        await LoadRandomDataSet(3);

        var group = Generator.Groups.First();

        var res = await Uow.Groups.Delete(group.Id);
        Uow.Save();

        res.Should().BeTrue();
        (Generator.Groups.Count - 1).Should().Be(Uow.Groups.Read().Count());
        (Generator.Subjects.Count - group.Subjects.Count).Should().Be(Uow.Subjects.Read().Count());
        (Generator.Couples.Count - group.Couples.Count).Should().Be(Uow.Couples.Read().Count());
    }


}