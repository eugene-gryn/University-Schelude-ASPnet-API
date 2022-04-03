namespace ScheduleLogic.UserLoad.Exception
{
    public class NotFoundProfileException : System.Exception
    {
        public NotFoundProfileException(string login, string password)
            : base($"User with {login} : {password} not found!")
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get; }
    }
}