using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SheldueLogic.SheldueObj;

namespace MainDesktop
{
    public class CouplesView
    {
        public CouplesView(Couple couple)
        {
            this.couple = couple;
        }

        public string Time { get { return couple.begin.ToString(@"hh\:mm") + " - " + couple.end.ToString(@"hh\:mm"); } }
        public string Name { get { 
                if (couple.subject.isPractice) return "* | " + couple.subject.Name;
                else return couple.subject.Name;
            } }
        public int Homework { get { return couple.homework; } }

        public Couple couple;
    }
}
