using System.Collections.Generic;
using SheldueLogic.User;

namespace SheldueLogic
{
    public class UsersList
    {
        private List<UserProfile> _users = new List<UserProfile>();
        private readonly IUserLoader loader;


        public UsersList(IUserLoader loader)
        {
            this.loader = loader;
        }

        public void LoadListOfUsers(IUserLoader loader)
        {
            _users = new List<UserProfile>(loader.GetUsers());
        }

        protected bool IsRegistered(UserProfile profile, string pass)
        {
            return !loader.GetUser(profile, pass).isNull();
        }

        public UserProfile GetRegisteredUser(UserProfile profile, string pass)
        {
            return loader.GetUser(profile, pass);
        }

        public bool Add(UserProfile profile, string pass)
        {
            return loader.RegisterNewUser(profile, pass);
        }

        public bool Login(UserProfile profile, string pass)
        {
            return IsRegistered(profile, pass);
        }
    }
}