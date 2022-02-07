using System.Collections.Generic;

namespace SheldueLogic
{
    public struct UserProfile
    {
        public string Login;
        public string Image;

        public List<SheldueObj.SubjectWeek> sheldues;


        public UserProfile(string login)
        {
            Login = login;
            Image = null;
            sheldues = new List<SheldueObj.SubjectWeek>();
        }

        public bool Equals(UserProfile obj)
        {
            return Login == obj.Login;
        }

        public bool isNull()
        {
            return string.IsNullOrEmpty(Login);
        }
    }
}
