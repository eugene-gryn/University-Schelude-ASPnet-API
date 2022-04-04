using System.Collections.Generic;
using ScheduleLogic.User;

namespace ScheduleLogic.UserLoad
{
    public interface ILoader
    {
        ICollection<UserProfile> GetUsers();
        UserProfile GetUser(string login, string password);
        void RegisterUser(UserProfile profile);
        bool isRegistered(string login);
    }
}