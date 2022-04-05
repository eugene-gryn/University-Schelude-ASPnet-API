using System;
using ScheduleLogic.Subject.Couples.Exception;

namespace ScheduleLogic.Subject.Couples
{
    public class Couple
    {
        public Couple(DateTime begin, DateTime end, Subject coupleSubject)
        {
            if (begin < end)
            {
                Begin = begin;
                End = end;
            }
            else
            {
                throw new WrongTimeCoupleException(coupleSubject);
            }

            CoupleSubject = coupleSubject;
        }

        public Couple()
        {
            CoupleSubject = null;
        }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public Subject CoupleSubject { get; set; }


        public bool IsEmpty()
        {
            return CoupleSubject != null &&
                   string.IsNullOrEmpty(CoupleSubject?.Name) &&
                   string.IsNullOrEmpty(CoupleSubject?.GoogleMeetUrl);
        }

        public bool IsCoupleFitInTime(DateTime time)
        {
            return Begin < time && time < End;
        }

        public bool Collision(Couple other)
        {
            return !(Begin <= other.Begin || End <= other.Begin);
        }

        public Couple Duplicate(DateTime begin, DateTime end)
        {
            return new Couple(begin, end, CoupleSubject);
        }
    }
}