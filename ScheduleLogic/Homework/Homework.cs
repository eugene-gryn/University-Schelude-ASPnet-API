using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleLogic.Homework
{
    public class HomeWork
    {
        public string LessonName { get; set; }
        public string HomeMade { get; set; }
        public bool IsCompleted { get; set; } = false;
        public byte HomeWorkPriority { get; set; }
        DateTime time = new DateTime();

        public HomeWork()
        {

        }
        public HomeWork(string lessonName, string homeMade)
        {
            LessonName = lessonName;
            HomeMade = homeMade;
        }

    }
}
