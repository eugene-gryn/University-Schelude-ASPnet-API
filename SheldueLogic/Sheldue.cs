using System;
using System.Collections.Generic;

namespace SheldueLogic
{
    public class Sheldue
    {
        public bool NotificatedBeforeCouple;
        public bool NotificatedAboutCouple;
        public bool NotificatedHomeworkCouple;

        public UserProfile profile = new UserProfile();
        private UsersList list = new UsersList(new TestLoader());   // NOT NESSESARY

        // Info about user
        public string GetProfileName => profile.Login;

        public string ImageIcon { get => profile.Image; set => profile.Image = value; }
        public List<SheldueObj.SubjectWeek> Sheldues { get => profile.sheldues; set => profile.sheldues = value; }



        public Sheldue()
        {
            Logged = false;
            list.LoadListOfUsers(new TestLoader());
            Sheldues = new List<SheldueObj.SubjectWeek>
            {
                new SheldueObj.SubjectWeek()
            };
            list = new UsersList(new TestLoader());
        }

        [Newtonsoft.Json.JsonConstructor]
        public Sheldue(string imageIcon, List<SheldueObj.SubjectWeek> sheldues, bool logged, UserProfile profile, UsersList list, bool notificatedBeforeCouple, bool notificatedAboutCouple, bool notificatedHomeworkCouple)
        {
            ImageIcon = imageIcon;
            Sheldues = sheldues;
            Logged = logged;
            this.profile = profile;
            this.list = new UsersList(new TestLoader());
            NotificatedBeforeCouple = notificatedBeforeCouple;
            NotificatedAboutCouple = notificatedAboutCouple;
            NotificatedHomeworkCouple = notificatedHomeworkCouple;

            if (Sheldues.Count == 0)
            {
                Sheldues.Add(new SheldueObj.SubjectWeek());
            }
        }

        public bool Login(string name, string pass)
        {
            if (list.Login(new UserProfile(name), pass))
            {
                profile = list.getRegisteredUser(new UserProfile(name), pass);
                Logged = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets couple based on parametr time
        /// </summary>
        /// <param name="weekDay">Whats week day of couple</param>
        /// <param name="time">Whats time need to be geted couple from</param>
        /// <returns>Couple near with time or empty couple if no availible</returns>
        public SheldueObj.Couple GetNearCouple(DayOfWeek weekDay, TimeSpan time)
        {
            SheldueObj.DaysOfWeek day = Sheldue.ConvertDaysOfWeek(weekDay);

            SheldueObj.Couple NearCouple = new SheldueObj.Couple(new TimeSpan(0), new TimeSpan(0), new SheldueObj.Subject("Нет ближайших предметов", false));

            SheldueObj.Day SheldueDay = Sheldues[CurrentWeek].days[(int)day];
            for (int i = 0; i < SheldueDay.Couples.Count; i++)
            {
                SheldueObj.Couple couple = SheldueDay.Couples[i];

                if (couple.isCoupleFitInTime(time) || (couple.begin > time))
                {
                    if (!string.IsNullOrEmpty(couple.subject.Name))
                    {
                        NearCouple = couple;
                        break;
                    }
                }
            }

            return NearCouple;
        }

        public void LoadSheldueFromExcel(SheldueObj.ISheldueConverter converter, string filename)
        {
            profile.sheldues = converter.GetSubjectWeek(filename);
        }

        /// <summary>
        /// Returns Week
        /// </summary>
        public int CurrentWeek
        {
            get
            {
                if (Sheldues.Count == 2)
                {
                    if (((DateTime.Now.DayOfYear + 3) / 7) % 2 != 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        public int PlanWeek(DateTime date)
        {
            if (Sheldues.Count == 2)
            {
                if (((date.DayOfYear + 3) / 7) % 2 != 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool Logged { get; private set; }

        public static SheldueObj.DaysOfWeek ConvertDaysOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return SheldueObj.DaysOfWeek.Sunday;
                case DayOfWeek.Monday:
                    return SheldueObj.DaysOfWeek.Monday;
                case DayOfWeek.Tuesday:
                    return SheldueObj.DaysOfWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return SheldueObj.DaysOfWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return SheldueObj.DaysOfWeek.Thursday;
                case DayOfWeek.Friday:
                    return SheldueObj.DaysOfWeek.Friday;
                case DayOfWeek.Saturday:
                    return SheldueObj.DaysOfWeek.Saturday;
                default:
                    return 0;
            }
        }
    }
}
