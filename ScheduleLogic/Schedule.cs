using System;
using System.ComponentModel;
using ScheduleLogic.Subject.Couples;
using ScheduleLogic.User;
using ScheduleLogic.UserLoad;
using ScheduleLogic.UserLoad.Exception;

namespace ScheduleLogic
{
    public class Schedule
    {
        private const int SCHELUDE_WEEKDAY_OFFSET = 1;


        public Schedule(ILoader loginer)
        {
            Logged = false;

            Loginer = loginer;
        }

        public UserProfile Profile { get; set; }
        public ILoader Loginer { get; set; }
        public bool Logged { get; private set; }


        /// <summary>
        ///     Returns Week
        /// </summary>
        public static int CurrentWeek()
        {
            return (DateTime.Now.DayOfYear + 3) / 7 % 2 != 0 ? 0 : 1;
        }

        public static DateTime FirstDayOnWeek()
        {
            // Day of week
            var offsetDay = 0 - (int) DateTime.Now.DayOfWeek + SCHELUDE_WEEKDAY_OFFSET;
            offsetDay += CurrentWeek() == 0 ? 0 : -7;

            return DateTime.Now.Date.AddDays(offsetDay);
        }

        public int PlanWeek(DateTime date)
        {
            return (date.DayOfYear + 3) / 7 % 2 == 0 ? 1 : 0;
        }

        public bool Login(string login, string password)
        {
            try
            {
                Profile = Loginer.GetUser(login, password);
                Logged = true;
                return true;
            }
            catch (NotFoundProfileException)
            {
                throw new NotFoundProfileException(login, password);
            }
        }

        public Couple NearCouple()
        {
        }

        public void Logout()
        {
            Profile = null;
            Logged = false;
        }
    }
}