namespace ScheduleLogic.User.Password.Default
{
    public static class DefaultCreator
    {
        public static PasswordHandler Create(string password)
        {
            return new DefaultPasswordHandler(password);
        }
    }
}