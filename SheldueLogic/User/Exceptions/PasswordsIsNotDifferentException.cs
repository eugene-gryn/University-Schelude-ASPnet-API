using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic.User.Exceptions
{
    internal class PasswordsIsNotDifferentException : Exception
    {
        public PasswordsIsNotDifferentException(string message) : base(message)
        {
        }
    }
}
