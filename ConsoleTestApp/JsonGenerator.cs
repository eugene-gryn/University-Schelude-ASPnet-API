using DAL.Entities;
using System;
using System.Linq;

namespace ConsoleTestApp
{
    public static class JsonGenerator
    {
        static Random random = new Random();
        static User user;
        static Group group;
        private static string StringGenerate(int length = 10)
        {
            const string chars =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private static int IdGenerate()
        {
            return random.Next(int.MaxValue);
        }

        private static bool BoolGenerate()
        {
            return random.Next(2) == 1;
        }
        private static byte[] ByteArraydGenerate()
        {
            byte[] result = new byte[20];
            random.NextBytes(result);
            return result;
        }
        
        public static List<Group> GroupsGenerate()
        {
            List<Group> groups = new List<Group>()
            { 
                GroupGenerate(),
                GroupGenerate(),
                GroupGenerate()
            };
            return groups;
        }
        public static List<Homework> HomerorksGenerate()
        {
            List<Homework> homeworks = new List<Homework>()
            {
                HomeworkGenerate(),
                HomeworkGenerate(),
                HomeworkGenerate()
            };
            return homeworks;
        }
        public static List<Subject> SubjectsGenerate()
        {
            List<Subject> subjects = new List<Subject>()
            {
                SubjectGenerate(),
                SubjectGenerate(),
                SubjectGenerate()
            };
            return subjects;
        }
        public static List<User> UsersGenerate()
        {
            List<User> users = new List<User>() 
            {
                UserGenerate(),
                UserGenerate(),
                UserGenerate()
            };
            return users;
        }
        public static List<Couple> CouplesGenerate()
        {
            List<Couple> couples = new List<Couple>()
            {
                CoupleGenerate(),
                CoupleGenerate(),
                CoupleGenerate()
            };
            return couples;
        }
        /*User*/
        public static User UserGenerate()
        {
            user = new User();
            user.Id = IdGenerate();
            user.Login = StringGenerate();
            user.Name = StringGenerate();
            user.IsAdmin = BoolGenerate();
            user.ImageLocation = StringGenerate();
            user.Password = ByteArraydGenerate();
            user.Salt = ByteArraydGenerate();
            user.Settings = SettingsGenerate();
            user.Groups = GroupsGenerate();
            user.Homeworks = HomerorksGenerate();
            return user;
        }
        /*Setting*/
        public static Settings SettingsGenerate()
        {
            Settings settings = new Settings();
            settings.NotifyBeforeCouple = BoolGenerate();
            settings.NotifyAboutCouple = BoolGenerate();
            settings.NotifyAboutHomework = BoolGenerate();
            settings.NotifyAboutDeadlineHomework = BoolGenerate();
            settings.NotifyAboutLoseDeadlineHomework = BoolGenerate();

            return settings;
        }
        /*Group*/
        public static Group GroupGenerate()
        {
            group = new Group();
            group.Id = IdGenerate();
            group.Name = StringGenerate(10);
            group.Creator = user;
            group.Subjects = SubjectsGenerate();
            group.Users = null;
            group.Couples = CouplesGenerate();
            return group;
        }
        /*Coupe*/
        public static Couple CoupleGenerate()
        {
            Couple couple = new Couple();
            couple.Id = IdGenerate();
            couple.Begin = DateTime.Now;
            couple.End = DateTime.Now.AddHours(1);
            couple.Subject = null;
            return couple;
        }

        /*Homework*/
        public static Homework HomeworkGenerate()
        {
            Homework homework = new Homework();
            homework.Id = IdGenerate();
            homework.Description = StringGenerate(10);
            homework.Deadline = TimeSpan.FromHours(random.Next(10,100));
            homework.Priority = (byte)random.Next(10);
            homework.Subject = null;
            return homework;
        }
        /*Subject*/
        public static Subject SubjectGenerate()
        {
            Subject subject = new Subject();    
            subject.Id = IdGenerate();
            subject.Name = StringGenerate(10);
            subject.IsPractice = BoolGenerate();
            subject.OwnerGroup = group;
            subject.Url = StringGenerate(10);
            subject.Location = StringGenerate(10);
            subject.Teacher = StringGenerate(10);
            return subject;
        }
    }
}
