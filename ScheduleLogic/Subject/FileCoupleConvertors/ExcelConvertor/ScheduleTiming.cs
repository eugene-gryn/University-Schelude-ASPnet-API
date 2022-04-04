using System;

namespace ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor
{
    public class ScheduleTiming
    {
        public int Size;
        public CoupleDefaultTime[] times;

        public ScheduleTiming()
        {
        }

        public ScheduleTiming(int size)
        {
            if (size > 0) Size = size;

            times = new CoupleDefaultTime[Size];
        }

        public ScheduleTiming(in ScheduleTiming other)
        {
            times = new CoupleDefaultTime[other.Size];
            for (var i = 0; i < other.Size; i++) times[i] = other.times[i];
        }

        public void SetTiming(int i, TimeSpan beg, TimeSpan end)
        {
            if (i < times.Length && i >= 0) times[i] = new CoupleDefaultTime(beg, end);
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