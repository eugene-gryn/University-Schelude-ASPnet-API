using System;
using System.Collections.Generic;
using ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor;
using System.Linq;

namespace ScheduleLogic.Subject.Couples
{
    public class CoupleManager
    {
        public CoupleManager(string name, SortedSet<Couple> couples)
        {
            Name = name;
            Couples = couples;
        }

        public CoupleManager(string name)
        {
            Name = name;
            Couples = new SortedSet<Couple>(new Couple());
        }

        public string Name { get; set; }
        public SortedSet<Couple> Couples { get; set; }
        public ScheduleTiming Timing { get; set; }

        public void Merge(CoupleManager other)
        {
            foreach (var couple in other.Couples)
            {
                if (!Couples.Add(couple)) throw new Exception.WrongTimeCoupleException(couple.CoupleSubject);
            }
        }

        public bool AddCouple(Couple couple) => Couples.Add(couple);

        public List<Couple> FindByDate(DateTime date)
        {
            return Couples.Where(couple => couple.Begin.Date == date.Date).ToList();
        }
    }
}