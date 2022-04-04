using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleLogic.Homework
{
    public class HomeWork
    {
        public Subject.Couples.Subject Subject { get; set; }
        public string HomeMade { get; set; }
        public bool IsCompleted { get; set; } = false;
        public byte HomeWorkPriority { get; set; }
        public DateTime DeadLine = new DateTime();

        public HomeWork()
        {

        }
        public HomeWork(Subject.Couples.Subject Subject, string homeMade, DateTime deadline, byte HomeWorkPriority = 5)
        {
            DeadLine = deadline;
            this.HomeWorkPriority = HomeWorkPriority;
            this.Subject = Subject;
            HomeMade = homeMade;
        }

    }
}
