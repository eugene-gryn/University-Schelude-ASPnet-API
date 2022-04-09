using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ScheduleLogic.Subject.Couples;
using ScheduleLogic.User;
using ScheduleLogic.UserLoad;
using ScheduleLogic.UserLoad.Exception;
using System.Windows.Threading;

namespace ScheduleLogic
{
    public class ScheduleEngine
    {
        private const int SCHELUDE_WEEKDAY_OFFSET = 1;

        public EventHandler CoupleChanged;

        public ScheduleEngine(ILoader loginer, Couple nearCouple)
        {
            Logged = false;

            Loginer = loginer;
            this.UserNearCouple = nearCouple;

            UserNearCouple = new Couple();
        }

        public UserProfile Profile { get; set; }
        public ILoader Loginer { get; set; }
        public bool Logged { get; private set; }

        public Couple UserNearCouple { get; set; }

        private DispatcherTimer _everySecondTimer = new DispatcherTimer();
        private bool _runned;


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
            var todayDates = Profile.UserSchelude.FindByDate(DateTime.Now.Date);
            var near = todayDates.First(couple => DateTime.Now < couple.End);

            if (UserNearCouple.CompareTo(near) != 0)
            {
                UserNearCouple = near;
                CoupleChanged.Invoke(this, EventArgs.Empty);
            }

            return UserNearCouple;
        }

        public void RunEngine(TimeSpan interval)
        {
            if (!_runned)
            {
                _everySecondTimer.Tick += _everySecondTimer_Tick;
                _everySecondTimer.Interval = interval;
                _everySecondTimer.Start();
            }
        }

        public void SuspendEngine()
        {
            if (_runned) _everySecondTimer.Stop();
        }

        private void _everySecondTimer_Tick(object sender, EventArgs e)
        {
            NearCouple();


        }

        public void Logout()
        {
            Profile = null;
            Logged = false;
        }
    }
}