namespace ScheduleLogic.User.Settings
{
    public class Settings
    {
        public bool NotifyBeforeCouple { get; set; }
        public bool NotifyAboutCouple { get; set; }
        public bool NotifyAboutHomework { get; set; }
        public bool NotifyAboutDeadlineHomework { get; set; }
        public bool NotifyAboutLoseDeadlineHomework { get; set; }

        public void DisableAll()
        {
            NotifyBeforeCouple = false;
            NotifyAboutCouple = false;
            NotifyAboutHomework = false;
            NotifyAboutDeadlineHomework = false;
            NotifyAboutLoseDeadlineHomework = false;
        }

        public void EnableAll()
        {
            NotifyBeforeCouple = true;
            NotifyAboutCouple = true;
            NotifyAboutHomework = true;
            NotifyAboutDeadlineHomework = true;
            NotifyAboutLoseDeadlineHomework = true;
        }
    }
}