using System;
using ScheduleLogic.User;
using ScheduleLogic.UserLoad;
using ScheduleLogic.UserLoad.Exception;

namespace ScheduleLogic
{
    public class Schedule
    {
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
        public void Logout()
        {
            Profile = null;
            Logged = false;
        }
    }
}