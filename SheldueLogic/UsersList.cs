using System.Collections.Generic;

namespace SheldueLogic
{
    public class UsersList
    {
        private IUserLoader loader;
        private List<UserProfile> users = new List<UserProfile>();


        public UsersList(IUserLoader loader)
        {
            this.loader = loader;
        }

        public void LoadListOfUsers(IUserLoader loader)
        {
            users = new List<UserProfile>(loader.GetUsers());
        }

        protected bool isRegistered(UserProfile profile, string pass)
        {
            return !loader.GetUser(profile, pass).isNull();
        }

        public UserProfile getRegisteredUser(UserProfile profile, string pass)
        {
            return loader.GetUser(profile, pass);
        }

        public bool Add(UserProfile profile, string pass)
        {
            return loader.RegisterNewUser(profile, pass);
        }

        public bool Login(UserProfile profile, string pass)
        {
            return isRegistered(profile, pass);
        }
    }
}
