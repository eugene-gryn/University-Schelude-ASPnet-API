using System.Collections.Generic;
using SheldueLogic.User;

namespace SheldueLogic.UserLoading
{
    public interface IUserLoader
    {
        ICollection<UserProfile> GetUsers();
        void RegisterNewUser(UserProfile profile);
        UserProfile GetUser(string login, string password);
    }


}