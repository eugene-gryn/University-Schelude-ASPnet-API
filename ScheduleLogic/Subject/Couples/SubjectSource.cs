using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleLogic.Subject.Couples
{
    public static class SubjectSource
    {
        public static HashSet<Subject> Subjects { get; set; }

        static SubjectSource()
        {
            Subjects = new HashSet<Subject>();
        }

        public static Subject GetSubject(string name, bool isPractice)
        {
            if (Subjects.Contains(new Subject(name, isPractice)))
            {

            }

            return null;
        }
    }
}
