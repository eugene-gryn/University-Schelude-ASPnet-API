using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using BLL.DTO.Models.UserModels.Password;
using DAL.EF;
using DAL.Entities;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using RandomDataGenerator.DataGenerator;
using SQLitePCL;

namespace BLL.DELETE;

public class TestWorker {
    public TestWorker() {
    }

    public async Task Run() {
        var context = new ScheduleContext(new ScheduleSqlLiteFactory("TestDB.sqlite"));
        IUnitOfWork uow = new EfUnitOfWork(context);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var gen = new RandomDataGenerator.DataGenerator.ScheduleRandomGenerator();

        var password = new PasswordHandler();

        password.CreatePasswordHash("password", out byte[] firstHash, out byte[] firstSalt);

        var users = gen.GenEmptyUsers(5);
        users.ForEach(u => {
            u.Login = "login" + u.Id;
            u.Password = firstHash;
            u.Salt = firstSalt;
        });
        await uow.Users.AddRange(users);
        uow.Save();


        gen.MakeDataSet(5);

        gen.Users.ForEach(u => {
            u.Login = "loginUser" + u.Id;
            u.Password = firstHash;
            u.Salt = firstSalt;
        });
        var firstAdmin = uow.Users.Read().First(u => u.Login.StartsWith("login"));
        firstAdmin.IsAdmin = true;
        await uow.Users.UpdateAsync(firstAdmin);
        uow.Save();

        await FillDbDataSet.FillUowData(uow, gen, refillGenerator: false);
    }
}