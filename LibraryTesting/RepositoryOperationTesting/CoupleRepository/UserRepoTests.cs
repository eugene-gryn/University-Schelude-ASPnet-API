using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

[TestFixture]
public class UserRepoTests : BaseRepositoryTest
{
    [Test]
    public async Task Creation_Successful()
    {
        var user = Generator.GenEmptyUsers(1).First();

        var result = await Uow.Users.Add(user);
        Uow.Save();

        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(1);
    }

    [Test]
    public async Task NotEmptyCreation_UnSuccessful()
    {
        int count = 4;
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
    public async Task RangeCreationWithoutDependencies_Successful()
    {
        var USERS_COUNT = 4;
        var users = Generator.GenEmptyUsers(USERS_COUNT);

        var result = await Uow.Users.AddRange(users);
        Uow.Save();


        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(users.Count);
    }

    [Test]
    public async Task Update_Successful()
    {
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
}