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
            Subjects = new HashSet<Subject>(new Subject("", false));
        }

        public static Subject GetSubject(string name, bool isPractice)
        {
            var containSubj = new Subject(name, isPractice);

            if (!Subjects.Add(containSubj))
            {
                return Subjects.First(subject => subject.Equals(subject, containSubj));
            }
            
            return containSubj;
        }
    }
}
