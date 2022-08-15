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
        await LoadRandomDataSet(3);
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

    [Test]
    public async Task AddUser_CreateUserAndAddToList_Successful() {
        await LoadRandomDataSet(3);
        var user = await AddUser();
        var group = Uow.Groups.Read().First();

        var result = await Uow.Groups.AddUser(group.Id, user.Id);
        Uow.Save();

        result.Should().BeTrue();
        var groupU = await Uow.Groups.ReadById(group.Id)
            .Include(g => g.UsersRoles)
            .SingleOrDefaultAsync();
        groupU.Should().NotBeNull();
        groupU.UsersRoles.Any(role => role.UserId == user.Id).Should().BeTrue();
    }

    [Test]
    public async Task RemoveUser_DeleteFromExistingGroup_SuccessfulErase() {
        await LoadRandomDataSet(3);
        var group = Uow.Groups.Read().Include(g => g.UsersRoles).First();
        var groupUsersCount = group.UsersRoles.Count;
        var userIdToDelete = group.UsersRoles.Last().UserId;

        var result = await Uow.Groups.RemoveUser(group.Id, userIdToDelete);
        Uow.Save();
        var groupU = await Uow.Groups.ReadById(group.Id).Include(g => g.UsersRoles).SingleOrDefaultAsync();

        result.Should().BeTrue();
        (groupUsersCount - 1).Should().Be(groupU!.UsersRoles.Count);
        groupU.UsersRoles.Any(role => role.UserId == userIdToDelete).Should().BeFalse();
    }
}