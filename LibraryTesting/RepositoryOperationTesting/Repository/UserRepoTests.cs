using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.Repository;

[TestFixture]
public class UserRepoTests : BaseRepositoryTest {
    [Test]
    public async Task Creation_Successful() {
        var user = Generator.GenEmptyUsers(1).First();

        var result = await Uow.Users.Add(user);
        Uow.Save();

        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(1);
    }

    [Test]
    public async Task NotEmptyCreation_InsertedNewEntityWithoutDependency_UnSuccessful() {
        var count = 4;
        Generator.MakeDataSet(count);
        var user = Generator.Users.FirstOrDefault();

        var result = await Uow.Users.AddRange(Generator.Users);
        Uow.Save();

        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(count);
        Uow.Users.Read()
            .Include(u => u.UsersRoles)
            .First().UsersRoles.Count.Should().Be(0);
    }

    [Test]
    public async Task RangeCreationWithoutDependencies_Successful() {
        var USERS_COUNT = 4;
        var users = Generator.GenEmptyUsers(USERS_COUNT);

        var result = await Uow.Users.AddRange(users);
        Uow.Save();


        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(users.Count);
    }



    [Test]
    public async Task Update_Successful() {
        await Creation_Successful();
        var user = await Uow.Users.Read().FirstOrDefaultAsync();
        var newName = "Team Kuk";

        user.Name = newName;
        var result = await Uow.Users.Update(user);
        var newUser = await Uow.Users.Read().FirstOrDefaultAsync(userF => userF.Id == user.Id);

        result.Should().BeTrue();
        newUser.Should().NotBeNull();
        newUser!.Name.Should().Be(user.Name);
    }

    [Test]
    public async Task Update_WithNotUniqueLogin_FalseReturn()
    {
        await GenerateRandomDataSet(3);
        var user1 = await Uow.Users.ReadById(1).SingleOrDefaultAsync();
        var user2 = await Uow.Users.ReadById(2).SingleOrDefaultAsync();

        user1!.Login = user2!.Login;

        var result = await Uow.Users.Update(user1);

        result.Should().BeFalse();
    }

    [Test]
    public async Task Delete_RemoveAllDependentEntities_Successful() {
        await GenerateRandomDataSet(1);
        var user = Uow.Users.Read()
            .Include(u => u.UsersRoles)
            .Include(u => u.Homework)
            .First();

        var result = await Uow.Users.Delete(user.Id);
        Uow.Save();

        result.Should().BeTrue();
        (Generator.Users.Count - 1).Should().Be(Uow.Users.Read().Count());
        (Generator.Homework.Count - user.Homework.Count).Should().Be(Uow.Homework.Read().Count());
    }

    [Test]
    public async Task Delete_RemoveAllDependentEntities_SuccessfulRemoveGroupDepend() {
        await GenerateRandomDataSet(2);
        var userUow = Uow.Users.Read()
            .First();

        var countCouplesToDelete = Uow.Users.ReadById(userUow.Id)
            .Include(u => u.UsersRoles)
            .ThenInclude(role => role.Group)
            .ThenInclude(g => g.Couples)
            .First().UsersRoles.Where(role => role.IsOwner).Select(role => role.Group.Couples.Count).Sum();
        var countSubjectsToDelete = Uow.Users.ReadById(userUow.Id)
            .Include(u => u.UsersRoles)
            .ThenInclude(role => role.Group)
            .ThenInclude(g => g.Subjects)
            .First().UsersRoles.Where(role => role.IsOwner).Select(role => role.Group.Subjects.Count).Sum();
        var countGroupsToDelete = userUow.UsersRoles.Where(r => r.IsOwner).ToList().Count;


        var result = await Uow.Users.Delete(userUow.Id);
        Uow.Save();


        result.Should().BeTrue();
        (Generator.Groups.Count - countGroupsToDelete).Should().Be(Uow.Groups.Read().Count());
        (Generator.Couples.Count - countCouplesToDelete).Should().Be(Uow.Couples.Read().Count());
        (Generator.Subjects.Count - countSubjectsToDelete).Should().Be(Uow.Subjects.Read().Count());
    }

    [Test]
    public async Task ReadById_ReadEntity_SuccessfulRead() {
        await GenerateRandomDataSet(2);


        var searchUser = Generator.Users.First();
        var foundUser = await Uow.Users.ReadById(searchUser.Id).FirstOrDefaultAsync();


        foundUser.Should().NotBeNull();
        foundUser.Name.Should().Be(searchUser.Name);
        foundUser.Login.Should().Be(searchUser.Login);
    }

    [Test]
    public async Task ReadById_ReadEntity_EmptyUser() {
        await GenerateRandomDataSet(2);


        var searchUser = Generator.Users.First();
        var foundUser = await Uow.Users.ReadById(searchUser.Id + 100).FirstOrDefaultAsync();


        foundUser.Should().BeNull();
    }
}