using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleLogic.Homework
{
    public class HomeWorkManagment
    {

        List<HomeWork> homeWorks;

        HomeWork homeWork = new HomeWork();
        //для сериалізації
        public HomeWorkManagment()
        {

        }
        //для створення домашки
        public HomeWorkManagment(DateTime deadline, byte HomeWorkPriority = 5)
        {
            Deadline = deadline;
            this.HomeWorkPriority = HomeWorkPriority;
        }
        //метод сортировки після визначення пріорітетності
        private void SortHomeWork(List<HomeWork> homeWorks)
        {
            var sorted = homeWorks.OrderBy(hw => hw.HomeWorkPriority);
            foreach (var hw in sorted)
                Console.WriteLine(hw);
        }
        public void ChangeHomeWork(List<HomeWork> homeWorks, int index, string homeMade)
        {
            homeWorks[index].HomeMade = homeMade;
        }
        public void DeleteHomeWork(List<HomeWork> homeWorks, int index)
        {
            homeWorks[index].Remove();
        }
        public void ChangeDeadLine(List<HomeWork> homeWorks, int index, DateTime date)
        {
            homeWorks[index].time = date;
        }
        public bool IsReadyHW()
        {
            if (homeWork.IsCompleted)
            {
                return true;
            }
            return homeWork.IsCompleted;
        }
    }
}
