using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic.SheldueObj
{
    public class SheldueTiming
    {
        public SheldueTiming()
        {

        }

        public SheldueTiming(int size)
        {
            if(size > 0) this.Size = size;
            times = new CoupleDefaultTime[Size];
        }
        public SheldueTiming(in SheldueTiming other)
        {
            times = new CoupleDefaultTime[other.Size];
            for (int i = 0; i < other.Size; i++)
            {
                this.times[i] = other.times[i];
            }
        }

        public void SetTiming(int i, TimeSpan beg, TimeSpan end)
        {
            if (i < times.Length && i >= 0) times[i] = new CoupleDefaultTime(beg, end);
        }

        public CoupleDefaultTime[] times;
        public int Size;
    }

    public struct CoupleDefaultTime
    {
        public TimeSpan starts;
        public TimeSpan ends;

        public CoupleDefaultTime(TimeSpan starts, TimeSpan ends)
        {
            this.starts = starts;
            this.ends = ends;
        }
    }
}
