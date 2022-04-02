using System.Collections.Generic;
using SheldueLogic.User;

namespace SheldueLogic
{
    public interface IUserLoader
    {
        ICollection<UserProfile> GetUsers();
        bool RegisterNewUser(UserProfile porfile, string passwork);
        UserProfile GetUser(UserProfile porfile, string password);
        bool isValidPassword(UserProfile porfile, string password);
    }


}