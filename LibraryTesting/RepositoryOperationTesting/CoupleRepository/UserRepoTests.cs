using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

[TestFixture]
public class UserRepoTests : BaseRepositoryTest
{
    [Test]
    public void UserCreationWithoutDependencies_SuccessfulAdd()
    {
        var user = Generator.RUser();


        Uow.Users.Add(user);
        Uow.Save();

        Uow.Users.Read().Count().Should().Be(1);
        Uow.Users.Read().First().Name.Should().Be(user.Name);
    }

    [Test]
    public void RangeUserCreationWithoutDependencies_SuccessfulAdd()
    {
        int USERS_COUNT = 4;
        var users = Generator.RUserList(USERS_COUNT);


        Uow.Users.AddRange(users);
        Uow.Save();

        Uow.Users.Read().Count().Should().Be(USERS_COUNT);
    }
}