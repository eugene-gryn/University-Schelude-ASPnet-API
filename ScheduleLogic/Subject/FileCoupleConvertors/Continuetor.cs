using System.Collections.Generic;
using ScheduleLogic.Subject.Couples;

namespace ScheduleLogic.Subject.FileCoupleConvertors
{
    public class Continuetor
    {
        private int _weekCount;
        private const int WeekSize = 7;

        public Continuetor(int weeks)
        {
            _weekCount = weeks;
        }

        public void Duplicate(CoupleManager manager, uint dublicateCount)
        {
            List<Couple> dublicated = new List<Couple>(manager.Couples);

            for (int i = 1; i < dublicateCount; i++)
            {
                foreach (var couple in dublicated)
                {
                    manager.Couples.Add(couple.Duplicate(couple.Begin.AddDays(_weekCount * WeekSize * i),
                        couple.End.AddDays(_weekCount * WeekSize * i)));
                }
            }
        }
    }
}