namespace SheldueLogic.User.Password
{
    public abstract class PasswordHandler
    {
        public object Password { get; set; }

        public abstract bool PasswordVerify(string password);
        public abstract object SetPassword(string password);
    }
}