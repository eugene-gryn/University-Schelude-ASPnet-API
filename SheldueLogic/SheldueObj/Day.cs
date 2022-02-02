using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic.SheldueObj
{
    public class Day
    {
        public Day()
        {
            for (int i = 0; i < 7; i++)
            {
                Couples.Add(new Couple());
            }
        }
        public Day(in Day other)
        {
            this.Couples = new List<Couple>(other.Couples);
        }

        public void DefineCouple(int num, TimeSpan begin, TimeSpan end, Subject subject)
        {
            Couples[num] = new Couple(begin, end, subject);
        }

        public List<Couple> Couples = new List<Couple>();
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
