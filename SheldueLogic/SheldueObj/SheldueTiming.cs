using System;

namespace SheldueLogic.SheldueObj
{
    public class SheldueTiming
    {
        public CoupleDefaultTime[] times;
        public int Size;

        public SheldueTiming()
        {

        }

        public SheldueTiming(int size)
        {
            if (size > 0)
            {
                Size = size;
            }

            times = new CoupleDefaultTime[Size];
        }
        public SheldueTiming(in SheldueTiming other)
        {
            times = new CoupleDefaultTime[other.Size];
            for (int i = 0; i < other.Size; i++)
            {
                times[i] = other.times[i];
            }
        }

        public void SetTiming(int i, TimeSpan beg, TimeSpan end)
        {
            if (i < times.Length && i >= 0)
            {
                times[i] = new CoupleDefaultTime(beg, end);
            }
        }
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
