using System;
using System.Collections.Generic;

namespace SheldueLogic.SheldueObj
{
    public class Day
    {
        public List<Couple> Couples = new List<Couple>();


        public Day()
        {
            for (var i = 0; i < 7; i++) Couples.Add(new Couple());
        }

        public Day(in Day other)
        {
            Couples = new List<Couple>(other.Couples);
        }


        public void DefineCouple(int num, TimeSpan begin, TimeSpan end, Subject subject)
        {
            Couples[num] = new Couple(begin, end, subject);
        }
    }

    public enum DaysOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
}