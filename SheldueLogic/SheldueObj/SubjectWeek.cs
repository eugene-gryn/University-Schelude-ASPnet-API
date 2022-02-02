using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic.SheldueObj
{
    public class SubjectWeek
    {
        public SubjectWeek()
        {
            WeekName = "";
            timing = new SheldueTiming(0);

            // Init days
            days = new Day[7];
            for (int i = 0; i < days.Length; i++)
            {
                days[i] = new Day();
            }
        }

        public SubjectWeek(string weekName, int CoupleCount)
        {
            WeekName = weekName;
            timing = new SheldueTiming(CoupleCount);

            // Init days
            days = new Day[7];
            for (int i = 0; i < days.Length; i++)
            {
                days[i] = new Day();
            }
        }

        public SubjectWeek(in SubjectWeek other)
        {
            for (int i = 0; i < other.days.Length; i++)
            {
                this.days[i] = new Day(other.days[i]);
            }
            this.timing = new SheldueTiming(other.timing);
        }

        public Day[] days;

        public string WeekName;

        public SheldueTiming timing;
    }
}
