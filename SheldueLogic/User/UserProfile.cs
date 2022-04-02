using System.Collections.Generic;
using SheldueLogic.SheldueObj;
using SheldueLogic.User.Exceptions;
using SheldueLogic.User.Password;

namespace SheldueLogic.User
{
    public class UserProfile
    {
        public UserProfile(string login, PasswordHandler password, string name,
            string image = "", Settings.Settings settings = null)
        {
            Login = login;
            Password = password;

            Name = name;

            ImageLocation = string.IsNullOrEmpty(image) ? image : DefaultValues.Image;
            Settings = settings ?? DefaultValues.settings;


            // OLD CODE
            Sheldues = new List<SubjectWeek>();
        }

        public string Login { get; set; }
        public string ImageLocation { get; set; }
        public string Name { get; set; }
        public PasswordHandler Password { get; set; }
        public Settings.Settings Settings { get; set; }

        // OLD CODE
        public List<SubjectWeek> Sheldues { get; set; }


        public void ChangePassword(string newPassword)
        {
            if (!Password.PasswordVerify(newPassword)) Password.SetPassword(newPassword);
            else throw new PasswordsIsNotDifferentException("The changed password is equal to last password");
        }
    }
}