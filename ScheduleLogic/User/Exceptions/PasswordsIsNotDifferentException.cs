using System;

namespace ScheduleLogic.User.Exceptions
{
    internal class PasswordsIsNotDifferentException : Exception
    {
        public PasswordsIsNotDifferentException(string message) : base(message)
        {
        }
    }
}
