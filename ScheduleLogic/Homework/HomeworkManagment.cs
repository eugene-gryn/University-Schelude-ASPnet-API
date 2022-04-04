using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleLogic.Homework
{
    public class HomeWorkManagment
    {
        List<HomeWork> HomeWorks;

        HomeWork homeWork = new HomeWork();
        //для сериалізації
        public HomeWorkManagment()
        {

        }
        //метод сортировки після визначення пріорітетності
        private void SortHomeWork(List<HomeWork> homeWorks)
        {
            var sorted = homeWorks.OrderBy(hw => hw.HomeWorkPriority);
            // Пофиксить(исправлено для запуска было -> HomeWork += hw;)
            foreach (var hw in sorted)
                HomeWorks.Add(hw);
        }
        public void ChangeHomeWork(List<HomeWork> homeWorks, int index, string homeMade)
        {
            homeWorks[index].HomeMade = homeMade;
        }
        public void DeleteHomeWork(List<HomeWork> homeWorks, int index)
        {
            homeWorks.RemoveAt(index);
        }
        public void ChangeDeadLine(List<HomeWork> homeWorks, int index, DateTime date)
        {
            homeWorks[index].DeadLine = date;
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
