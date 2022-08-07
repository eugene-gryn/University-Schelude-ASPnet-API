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
        CreateUser();
        var user = Generator.Users.FirstOrDefault();

        var result = await Uow.Users.Add(user);
        Uow.Save();

        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(Generator.Users.Count);
    }

    [Test]
    public async Task NotEmptyCreation_UnSuccessful()
    {
        Generator.RDataSet(5);
        var user = Generator.Users.FirstOrDefault();

        var result = await Uow.Users.Add(user);
        Uow.Save();

        result.Should().Be(false);
        Uow.Users.Read().Count().Should().Be(0);
    }

    [Test]
    public async Task RangeCreationWithoutDependencies_Successful()
    {
        var USERS_COUNT = 4;
        CreateUser(USERS_COUNT);

        var result = await Uow.Users.AddRange(Generator.Users);
        Uow.Save();

        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(Generator.Users.Count);
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

        result.Should().Be(true);
        newUser.Should().NotBeNull();
        newUser!.Name.Should().Be(user.Name);
    }
}