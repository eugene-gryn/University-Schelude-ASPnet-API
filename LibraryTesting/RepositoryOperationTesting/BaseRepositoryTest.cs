using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RandomDataGenerator.DataGenerator;

namespace LibraryTesting.RepositoryOperationTesting;

[TestFixture]
public class BaseRepositoryTest : BaseTest {
    protected override void EveryTimeSetUp() {
        base.EveryTimeSetUp();

        Generator.Clear();
    }

    public BaseRepositoryTest() {
        Generator = new ScheduleRandomGenerator();
    }

    protected ScheduleRandomGenerator Generator { get; }


    protected async Task GenerateRandomDataSet(int countUser = 1) {
        Generator.MakeDataSet(countUser);

        await FillDbDataSet.FillUowData(Uow, Generator);
    }

    protected User RegisterNewUser() {
        var user = Generator.GenEmptyUsers(1).First();
        Uow.Users.Add(ref user);
        return user;
    }
    protected async Task<User> AddUser() {
        var user = Generator.GenEmptyUsers(1).First();

        var addResult = await Uow.Users.Add(user);

        if (!addResult) return null;

        Uow.Save();

        user = Uow.Users.Read().Last();

        return user;
    }
}