using SheldueLogic.SheldueObj;

namespace MainDesktop
{
    public class CouplesView
    {
        public CouplesView(Couple couple)
        {
            this.couple = couple;
        }

        public string Time => couple.begin.ToString(@"hh\:mm") + " - " + couple.end.ToString(@"hh\:mm");
        public string Name
        {
            get
            {
                if (couple.subject.isPractice)
                {
                    return "* | " + couple.subject.Name;
                }
                else
                {
                    return couple.subject.Name;
                }
            }
        }
        public int Homework => couple.HomeworkCount;

        public Couple couple;
    }
}
