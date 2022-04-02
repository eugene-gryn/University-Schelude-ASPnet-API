using System.Collections.Generic;

namespace SheldueLogic
{
    public interface IUserLoader
    {
        ICollection<UserProfile> GetUsers();
        bool RegisterNewUser(UserProfile porfile, string passwork);
        UserProfile GetUser(UserProfile porfile, string password);
        bool isValidPassword(UserProfile porfile, string password);
    }

    public class TestLoader : IUserLoader
    {
        public UserProfile GetUser(UserProfile porfile, string password)
        {
            if (porfile.Login == "test" && password == "test")
                return new UserProfile("test");
            return new UserProfile("");
        }

        public ICollection<UserProfile> GetUsers()
        {
            var users = new List<UserProfile>
            {
                new UserProfile("test")
            };
            return users;
        }

        public bool isValidPassword(UserProfile porfile, string password)
        {
            return porfile.Login == "test" && password == "test";
        }

        public bool RegisterNewUser(UserProfile porfile, string password)
        {
            return !GetUser(porfile, password).isNull();
        }
    }
}