using System;
using System.Threading;

namespace ScheduleLogic.Subject.Couples.Exception
{
    public class WrongTimeCoupleException : System.Exception
    {
        public Subject CoupleSubject { get; }

        public WrongTimeCoupleException(Subject coupleSubject)
            : base($"{coupleSubject.Name} has a wrong time on initialization")
        {
            CoupleSubject = coupleSubject;
        }
    }
}
