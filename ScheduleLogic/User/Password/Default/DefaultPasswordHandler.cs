namespace ScheduleLogic.User.Password.Default
{
    public class DefaultPasswordHandler : PasswordHandler
    {
        public DefaultPasswordHandler(string password)
        {
            Password = SetPassword(password);
        }

        public DefaultPasswordHandler()
        {
        }

        public override bool PasswordVerify(string password)
        {
            return password.Equals(Password);
        }

        public override object SetPassword(string password)
        {
            return password.GetHashCode();
        }
    }
}