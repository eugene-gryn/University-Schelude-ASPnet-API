using System.Collections.Generic;
using ScheduleLogic.User;
using ScheduleLogic.User.Password.Default;
using ScheduleLogic.UserLoad.Exception;

namespace ScheduleLogic.UserLoad.TestLoader
{
    public class Test : ILoader
    {
        private readonly List<UserProfile> _list;

        public Test()
        {
            _list = new List<UserProfile>();
            _list.Add(new UserProfile("test", DefaultCreator.Create("test"), "Test account"));
        }

        public ICollection<UserProfile> GetUsers()
        {
            return _list;
        }

        public UserProfile GetUser(string login, string password)
        {
            var users = GetUsers();

            foreach (var user in users)
                if (user.Login == login && user.Password.PasswordVerify(password))
                    return user;

            throw new NotFoundProfileException(login, password);
        }

        public void RegisterUser(UserProfile profile)
        {
            GetUsers().Add(profile);
        }

        public bool isRegistered(string login)
        {
            var users = GetUsers();

            foreach (var user in users)
                if (user.Login == login)
                    return true;

            return false;
        }
    }
}