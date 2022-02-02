using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic
{
    public struct UserProfile
    {
        public string Login;

        public string Image;

        public UserProfile(string login)
        {
            Login = login;
            Image = null;
            sheldues = new List<SheldueObj.SubjectWeek>();
        }

        public bool Equals(UserProfile obj) => this.Login == obj.Login;

        public bool isNull() => String.IsNullOrEmpty(Login);

        public List<SheldueObj.SubjectWeek> sheldues;
    }
}
