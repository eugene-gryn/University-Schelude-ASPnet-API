namespace ScheduleLogic.User
{
    public static class DefaultValues
    {
        public static string Image = "";

        public static Settings.Settings settings = new Settings.Settings()
        {
            NotyfiAboutCouple = true,
            NotifyBeforeCouple = true,
            NotifyAboutHomework = true, 
            NotifyAboutDeadlineHomework = true,
            NotifyAboutLoseDeadlineHomework = true
        };
    }
}
