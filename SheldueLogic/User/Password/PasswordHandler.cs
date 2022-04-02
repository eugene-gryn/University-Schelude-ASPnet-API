using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic.User.Password
{
    public abstract class PasswordHandler
    {
        public object Password { get; set; }

        protected PasswordHandler()
        {
        }

        public abstract bool PasswordVerify(string password);
        public abstract object SetPassword(string password);
    }
}
