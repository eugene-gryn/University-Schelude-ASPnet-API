using System.Collections.Generic;
using SheldueLogic.User;

namespace SheldueLogic.UserLoad
{
    public interface ILoader
    {
        ICollection<UserProfile> GetUsers();
        UserProfile GetUser(string login, string password);
        void RegisterUser(UserProfile profile);
    }
}