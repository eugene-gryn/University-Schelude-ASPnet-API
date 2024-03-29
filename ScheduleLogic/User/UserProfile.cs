﻿using System.Collections.Generic;
using ScheduleLogic.Subject;
using ScheduleLogic.Subject.Couples;
using ScheduleLogic.User.Exceptions;
using ScheduleLogic.User.Password;

namespace ScheduleLogic.User
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

            Couples = new List<Couple>();
        }

        public string Login { get; set; }
        public string ImageLocation { get; set; }
        public string Name { get; set; }
        public PasswordHandler Password { get; set; }
        public Settings.Settings Settings { get; set; }

        public List<Couple> Couples;

        public void ChangePassword(string newPassword)
        {
            if (!Password.PasswordVerify(newPassword)) Password.SetPassword(newPassword);
            else throw new PasswordsIsNotDifferentException("The changed password is equal to last password");
        }
    }
}