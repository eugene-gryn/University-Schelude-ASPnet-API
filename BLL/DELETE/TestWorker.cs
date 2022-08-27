using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using BLL.DTO.Models.UserModels.Password;
using DAL.EF;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using RandomDataGenerator.DataGenerator;

namespace BLL.DELETE;

public class TestWorker {
    public TestWorker() {
    }

    public async Task Run() {
        var context = new ScheduleContext(new ScheduleSqlLiteFactory("TestDB"));
        IUnitOfWork uow = new EfUnitOfWork(context);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var gen = new RandomDataGenerator.DataGenerator.ScheduleRandomGenerator();

        var password = new PasswordHandler();

        password.CreatePasswordHash("password", out byte[] firstHash, out byte[] firstSalt);

        gen.MakeDataSet(8);

        gen.Users.ForEach(u => {
            u.Login = "loginUser" + u.Id;
            u.Password = firstHash;
            u.Salt = firstSalt;
        });

        var users = gen.GenEmptyUsers(5);
        users.ForEach(u => {
            u.Login = "login" + u.Id;
            u.Password = firstHash;
            u.Salt = firstSalt;
        });
        await uow.Users.AddRange(users);
        uow.Save();

        var firstAdmin = uow.Users.Read().First(u => u.Login.StartsWith("login"));
        firstAdmin.IsAdmin = true;
        await uow.Users.Update(firstAdmin);
        uow.Save();

        await FillDbDataSet.FillUowData(uow, gen);
    }
}