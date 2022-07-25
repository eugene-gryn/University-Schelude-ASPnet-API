using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository;

[TestFixture]
public class UserRepoTests : BaseRepositoryTest
{
    [Test]
    public void UserCreation_SuccessfulAdd()
    {
        Generator.RGenerate(10);

        foreach (var user in Generator.Users) Uow.Users.Create(user);
        Uow.Save();
        UowUpdate();

        Uow.Users.Read().Count().Should().Be(Generator.Users.Count);
    }
}