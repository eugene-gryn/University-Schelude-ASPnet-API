using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            {
                return new UserProfile("test");
            }
            else return new UserProfile("");
        }

        public ICollection<UserProfile> GetUsers()
        {
            List <UserProfile> users = new List<UserProfile>();
            users.Add(new UserProfile("test"));
            return users;
        }

        public bool isValidPassword(UserProfile porfile, string password) => porfile.Login == "test" && password == "test";

        public bool RegisterNewUser(UserProfile porfile, string password) => !GetUser(porfile, password).isNull();
    }
}
