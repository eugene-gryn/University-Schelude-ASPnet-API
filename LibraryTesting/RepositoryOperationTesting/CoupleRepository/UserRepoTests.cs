using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

[TestFixture]
public class UserRepoTests : BaseRepositoryTest
{
    [Test]
    public async Task UserCreation_Successful()
    {
        CreateUser();
        var user = Generator.Users.FirstOrDefault();

        var result = await Uow.Users.Add(user);
        Uow.Save();

        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(Generator.Users.Count);
    }

    [Test]
    public async Task UserNotEmptyCreation_UnSuccessful()
    {
        Generator.RGenerate(2);
        var user = Generator.Users.FirstOrDefault();

        var result = await Uow.Users.Add(user);
        Uow.Save();

        result.Should().Be(false);
        Uow.Users.Read().Count().Should().Be(0);
    }

    [Test]
    public async Task RangeUserCreationWithoutDependencies_Successful()
    {
        int USERS_COUNT = 4;
        CreateUsers(4);

        var result = await Uow.Users.AddRange(Generator.Users);
        Uow.Save();

        result.Should().Be(true);
        Uow.Users.Read().Count().Should().Be(Generator.Users.Count);
    }

    [Test]
    public async Task UpdateUser_Successful()
    {
        await UserCreation_Successful();
        var user = await Uow.Users.Read().FirstOrDefaultAsync();
        var newName = "Team Kuk";


        user.Name = newName;
        var result = await Uow.Users.Update(user);
        var newUser = await Uow.Users.Read().FirstOrDefaultAsync(userF => userF.Id == user.Id);

        result.Should().Be(true);
        newUser.Should().NotBeNull();
        newUser.Name.Should().Be(user.Name);
    }
}