using System.Collections.Generic;
using SheldueLogic.User;
using SheldueLogic.User.Password.Default;
using SheldueLogic.UserLoad.Exception;

namespace SheldueLogic.UserLoad.TestLoader
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
            _list.Add(profile);
        }
    }
}