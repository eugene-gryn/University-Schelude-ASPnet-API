using ScheduleLogic.Subject.Couples;

namespace MainDesktop
{
    public class CouplesView
    {
        public Couple couple;

        public CouplesView(Couple couple)
        {
            this.couple = couple;
        }

        public string Time => couple.Begin.ToString(@"hh\:mm") + " - " + couple.End.ToString(@"hh\:mm");

        public string Name
        {
            get
            {
                if (couple.CoupleSubject.isPractice)
                    return "* | " + couple.CoupleSubject.Name;
                return couple.CoupleSubject.Name;
            }
        }

    }
}