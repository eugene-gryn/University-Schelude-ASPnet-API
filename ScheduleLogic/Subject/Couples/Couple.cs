using System;
using System.Collections.Generic;
using ScheduleLogic.Subject.Couples.Exception;

namespace ScheduleLogic.Subject.Couples
{
    public class Couple : IComparable<Couple>, IComparer<Couple>
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

        public int CompareTo(Couple other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            if (Collision(other)) return 0;

            var beginComparison = Begin.CompareTo(other.Begin);
            if (beginComparison != 0) return beginComparison;
            return End.CompareTo(other.End);
        }

        public int Compare(Couple x, Couple y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            if (x.Collision(y)) return 0;

            var beginComparison = x.Begin.CompareTo(y.Begin);
            if (beginComparison != 0) return beginComparison;
            return x.End.CompareTo(y.End);
        }


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