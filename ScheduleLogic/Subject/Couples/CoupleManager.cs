using System.Collections.Generic;
using System.Collections.Specialized;

namespace ScheduleLogic.Subject.Couples
{
    public class CoupleManager
    {
        public string Name { get; set; }
        public List<Couple> Couples { get; set; }


        public CoupleManager(string name, List<Couple> couples)
        {
            Name = name;
            Couples = couples;
        }


    }
}