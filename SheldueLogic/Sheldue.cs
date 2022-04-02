using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SheldueLogic.SheldueObj;

namespace SheldueLogic
{
    public class Sheldue
    {
        private readonly UsersList list = new UsersList(new TestLoader()); // NOT NESSESARY
        public bool NotificatedAboutCouple;
        public bool NotificatedBeforeCouple;
        public bool NotificatedHomeworkCouple;

        public UserProfile profile;


        public Sheldue()
        {
            Logged = false;
            list.LoadListOfUsers(new TestLoader());
            Sheldues = new List<SubjectWeek>
            {
                new SubjectWeek()
            };
            list = new UsersList(new TestLoader());
        }

        [JsonConstructor]
        public Sheldue(string imageIcon, List<SubjectWeek> sheldues, bool logged, UserProfile profile, UsersList list,
            bool notificatedBeforeCouple, bool notificatedAboutCouple, bool notificatedHomeworkCouple)
        {
            ImageIcon = imageIcon;
            Sheldues = sheldues;
            Logged = logged;
            this.profile = profile;
            this.list = new UsersList(new TestLoader());
            NotificatedBeforeCouple = notificatedBeforeCouple;
            NotificatedAboutCouple = notificatedAboutCouple;
            NotificatedHomeworkCouple = notificatedHomeworkCouple;

            if (Sheldues.Count == 0) Sheldues.Add(new SubjectWeek());
        }

        // Info about user
        public string GetProfileName => profile.Login;

        public string ImageIcon
        {
            get => profile.Image;
            set => profile.Image = value;
        }

        public List<SubjectWeek> Sheldues
        {
            get => profile.Sheldues;
            set => profile.Sheldues = value;
        }

        /// <summary>
        ///     Returns Week
        /// </summary>
        public int CurrentWeek
        {
            get
            {
                if (Sheldues.Count == 2)
                {
                    if ((DateTime.Now.DayOfYear + 3) / 7 % 2 != 0)
                        return 0;
                    return 1;
                }

                return 0;
            }
        }

        public bool Logged { get; private set; }

        public bool Login(string name, string pass)
        {
            if (list.Login(new UserProfile(name), pass))
            {
                profile = list.GetRegisteredUser(new UserProfile(name), pass);
                Logged = true;
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets couple based on parametr time
        /// </summary>
        /// <param name="weekDay">Whats week day of couple</param>
        /// <param name="time">Whats time need to be geted couple from</param>
        /// <returns>Couple near with time or empty couple if no availible</returns>
        public Couple GetNearCouple(DayOfWeek weekDay, TimeSpan time)
        {
            var day = ConvertDaysOfWeek(weekDay);

            var NearCouple = new Couple(new TimeSpan(0), new TimeSpan(0),
                new Subject("Нет ближайших предметов", false));

            var SheldueDay = Sheldues[CurrentWeek].days[(int) day];
            for (var i = 0; i < SheldueDay.Couples.Count; i++)
            {
                var couple = SheldueDay.Couples[i];

                if (couple.isCoupleFitInTime(time) || couple.begin > time)
                    if (!string.IsNullOrEmpty(couple.subject.Name))
                    {
                        NearCouple = couple;
                        break;
                    }
            }

            return NearCouple;
        }

        public void LoadSheldueFromExcel(ISheldueConverter converter, string filename)
        {
            profile.Sheldues = converter.GetSubjectWeek(filename);
        }

        public int PlanWeek(DateTime date)
        {
            if (Sheldues.Count == 2)
            {
                if ((date.DayOfYear + 3) / 7 % 2 != 0)
                    return 0;
                return 1;
            }

            return 0;
        }

        public static DaysOfWeek ConvertDaysOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return DaysOfWeek.Sunday;
                case DayOfWeek.Monday:
                    return DaysOfWeek.Monday;
                case DayOfWeek.Tuesday:
                    return DaysOfWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return DaysOfWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return DaysOfWeek.Thursday;
                case DayOfWeek.Friday:
                    return DaysOfWeek.Friday;
                case DayOfWeek.Saturday:
                    return DaysOfWeek.Saturday;
                default:
                    return 0;
            }
        }
    }
}