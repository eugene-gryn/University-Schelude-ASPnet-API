using System.Collections.Generic;
using ScheduleLogic.Subject.Couples;

namespace ScheduleLogic.Subject.FileCoupleConvertors
{
    public interface IConverter
    {
        List<Couple> GetSubjectWeek(string filename);
    }
}