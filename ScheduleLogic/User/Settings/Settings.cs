namespace ScheduleLogic.User.Settings
{
    public class Settings
    {
        public bool NotifyBeforeCouple { get; set; }
        public bool NotyfiAboutCouple { get; set; }
        public bool NotifyAboutHomework { get; set; }
        public bool NotifyAboutDeadlineHomework { get; set; }
        public bool NotifyAboutLoseDeadlineHomework { get; set; }

        public void DisableAll()
        {
            NotifyBeforeCouple = false;
            NotyfiAboutCouple = false;
            NotifyAboutHomework = false;
            NotifyAboutDeadlineHomework = false;
            NotifyAboutLoseDeadlineHomework = false;
        }

        public void EnableAll()
        {
            NotifyBeforeCouple = true;
            NotyfiAboutCouple = true;
            NotifyAboutHomework = true;
            NotifyAboutDeadlineHomework = true;
            NotifyAboutLoseDeadlineHomework = true;
        }
    }
}