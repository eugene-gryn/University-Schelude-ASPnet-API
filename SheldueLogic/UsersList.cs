using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic
{
    public class UsersList
    {
        public UsersList(IUserLoader loader)
        {
            this.loader = loader;
        }

        public void LoadListOfUsers(IUserLoader loader)
        {
            users = new List<UserProfile>(loader.GetUsers());
        }

        protected bool isRegistered(UserProfile profile, string pass) => !loader.GetUser(profile, pass).isNull();

        public UserProfile getRegisteredUser(UserProfile profile, string pass) => loader.GetUser(profile, pass);

        public bool Add(UserProfile profile, string pass) => loader.RegisterNewUser(profile, pass);

        public bool Login(UserProfile profile, string pass) => isRegistered(profile, pass);

        IUserLoader loader;
        List<UserProfile> users = new List<UserProfile>();
    }
}
