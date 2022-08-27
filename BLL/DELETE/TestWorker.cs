using System.Runtime.ExceptionServices;
using BLL.DTO.Models.UserModels.Password;
using DAL.EF;
using DAL.Entities;
using DAL.UOW;

namespace BLL.DELETE;

public class TestWorker {
    public TestWorker() {
        var context = new ScheduleContext(new ScheduleSqlLiteFactory("TestDB"));
        IUnitOfWork uow = new EfUnitOfWork(context);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var gen = new RandomDataGenerator.DataGenerator.ScheduleRandomGenerator();

        var password = new PasswordHandler();

        password.CreatePasswordHash("password", out byte[] firstHash, out byte[] firstSalt);

        var users = gen.GenEmptyUsers(5);
        users.ForEach(u => {
            u.Login = "login" + u.Id;
            u.Password = firstHash;
            u.Salt = firstSalt;
        });

        uow.Users.AddRange(users);

        uow.Save();

        var userAdmin = uow.Users.Read().First();
        userAdmin.IsAdmin = true;
        uow.Users.Update(userAdmin);
        uow.Save();
    }
}