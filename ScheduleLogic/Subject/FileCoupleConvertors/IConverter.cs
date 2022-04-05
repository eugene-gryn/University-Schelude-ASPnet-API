using System.Collections.Generic;
using ScheduleLogic.Subject.Couples;

namespace ScheduleLogic.Subject.FileCoupleConvertors
{
    public interface IConverter
    {
        CoupleManager GetSubjectWeek(string name, string filename);
    }
}