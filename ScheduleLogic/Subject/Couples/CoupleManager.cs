using System.Collections.Generic;
using System.Collections.Specialized;
using ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor;

namespace ScheduleLogic.Subject.Couples
{
    public class CoupleManager
    {
        public string Name { get; set; }
        public List<Couple> Couples { get; set; }
        public ScheduleTiming Timing { get; set; }

        public CoupleManager(string name, List<Couple> couples)
        {
            Name = name;
            Couples = couples;
        }


    }
}